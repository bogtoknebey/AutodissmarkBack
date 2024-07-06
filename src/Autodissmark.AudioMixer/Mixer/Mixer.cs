using Autodissmark.AudioMixer.Mixer.DTO;
using Autodissmark.Domain.Options;
using Microsoft.Extensions.Options;
using NAudio.Wave;
using System.Diagnostics;
using System.Reflection;

namespace Autodissmark.AudioMixer.Mixer;

public class Mixer : IMixer
{
    private const int TargetSampleRate = 48000;
    private readonly string _tempPath;
    private readonly string _ffmpegPath;

    public Mixer(
        IOptions<FilePathOptions> filePathOptions,
        IOptions<FFmpegOptions> FFmpegOptions
    )
    {
        _tempPath = filePathOptions.Value.TempPath;
        _ffmpegPath = FFmpegOptions.Value.Path;
    }

    private async Task RunFFmpegCommand(string arguments)
    {
        using (var process = new Process())
        {
            process.StartInfo.FileName = "ffmpeg";
            process.StartInfo.WorkingDirectory = _ffmpegPath;
            process.StartInfo.Arguments = arguments;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.Start();
            process.WaitForExit();
        }
    }

    private async Task CompressAudioMP3(string filePath)
    {
        string outputFilePath = $"{filePath.Split('.')[0]}.mp3";
        await RunFFmpegCommand($"-i {filePath} -q:a 0 -map a {outputFilePath}");

        File.Delete(filePath);
        File.Move(outputFilePath, filePath);
        File.Delete(outputFilePath);
    }

    private async Task MixWavFiles(BaseMixFileDTO baseWavMixFile, MixFileDTO additionalWavMixFile, string outputFilePath)
    {
        using (var reader1 = new AudioFileReader(baseWavMixFile.FilePath))
        using (var reader2 = new AudioFileReader(additionalWavMixFile.FilePath))
        {
            var minLength = (int)Math.Min(reader1.Length, reader2.Length);

            reader1.Volume = baseWavMixFile.Volume;
            reader2.Volume = additionalWavMixFile.Volume;

            var waveFormat = reader1.WaveFormat;
            using (var writer = new WaveFileWriter(outputFilePath, waveFormat))
            {
                var buffer1 = new float[waveFormat.SampleRate * waveFormat.Channels];
                var buffer2 = new float[waveFormat.SampleRate * waveFormat.Channels];
                var mixedBuffer = new float[waveFormat.SampleRate * waveFormat.Channels];

                while (reader1.Position < minLength && reader2.Position < minLength)
                {
                    var bytesRead1 = reader1.Read(buffer1, 0, buffer1.Length);
                    var bytesRead2 = reader2.Read(buffer2, 0, buffer2.Length);
                    for (int i = 0; i < bytesRead1; i++)
                    {
                        mixedBuffer[i] = buffer1[i] + buffer2[i];
                    }
                    writer.WriteSamples(mixedBuffer, 0, bytesRead1);
                }
            }
        }
    }

    public async Task MixFiles(BaseMixFileDTO baseMixFile, MixFileDTO additionalMixFile, string outputFilePath, CancellationToken ct)
    {
        var tempBaseMixFilePath = Path.Combine(_tempPath, $"{Guid.NewGuid()}.wav");
        var tempAdditionalMixFilePath = Path.Combine(_tempPath, $"{Guid.NewGuid()}.wav");

        // Convert to wav
        await RunFFmpegCommand($"-i {baseMixFile.FilePath} -ac 2 -ar {TargetSampleRate} {tempBaseMixFilePath}");
        await RunFFmpegCommand($"-i {additionalMixFile.FilePath} -ac 2 -ar {TargetSampleRate} {tempAdditionalMixFilePath}");

        // Mix
        var baseWavMixFile = new BaseMixFileDTO(tempBaseMixFilePath, baseMixFile.Volume);
        var additionalWavMixFile = new MixFileDTO(tempAdditionalMixFilePath, additionalMixFile.Volume);

        await MixWavFiles(baseWavMixFile, additionalWavMixFile, outputFilePath);
        await CompressAudioMP3(outputFilePath);

        // Delete temp files
        File.Delete(tempBaseMixFilePath);
        File.Delete(tempAdditionalMixFilePath);
    }
}

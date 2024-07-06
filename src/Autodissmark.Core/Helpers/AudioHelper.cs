using Microsoft.AspNetCore.Http;
using NAudio.Wave;

namespace Autodissmark.Core.Helpers;

public static class AudioHelper
{
    public static async Task<int> GetAudioDurationAsync(IFormFile audioFile)
    {
        // TODO: fix (now its always - 0)
        var tempFilePath = Path.GetTempFileName();
        using (var stream = new FileStream(tempFilePath, FileMode.Create))
        {
            await audioFile.CopyToAsync(stream);
        }

        using (var audioFileReader = new AudioFileReader(tempFilePath))
        {
            var durationInSeconds = audioFileReader.TotalTime.TotalSeconds;
            return (int)(durationInSeconds * 1000);
        }
    }

    public static async Task<int> GetAudioDurationAsync(byte[] audioData)
    {
        return 0;
    }
}

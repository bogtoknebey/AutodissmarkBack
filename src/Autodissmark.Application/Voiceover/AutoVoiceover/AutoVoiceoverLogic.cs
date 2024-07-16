using Autodissmark.Application.Voiceover.AutoVoiceover.DTO;
using Autodissmark.ApplicationDataAccess.Repositories.ReadRepositories.Contracts;
using Autodissmark.ApplicationDataAccess.Repositories.WriteRepositories.Contracts;
using Autodissmark.Core.Helpers;
using Autodissmark.Domain.ApplicationModels;
using Autodissmark.Domain.Options;
using Autodissmark.ExternalServices.TextToSpeach.Contracts;
using Autodissmark.ExternalServices.TextToSpeach.DTO;
using Microsoft.Extensions.Options;

namespace Autodissmark.Application.Voiceover.AutoVoiceover;

public class AutoVoiceoverLogic : IAutoVoiceoverLogic
{
    private readonly string _acapellasPath;
    private readonly IAcapellaWriteRepository _writeRepository;
    private readonly ITextReadRepository _textReadRepository;
    private readonly Func<string, ITextToSpeach> _ttsChooser;
    private readonly IVoiceReadRepository _voiceReadRepository;

    public AutoVoiceoverLogic(
        IOptions<FilePathOptions> filePathOptions,
        IAcapellaWriteRepository writeRepository,
        ITextReadRepository textReadRepository,
        Func<string, ITextToSpeach> ttsChooser,
        IVoiceReadRepository voiceReadRepository)
    {
        _writeRepository = writeRepository;
        _acapellasPath = filePathOptions.Value.AcapellasFolderPath;
        _textReadRepository = textReadRepository;
        _ttsChooser = ttsChooser;
        _voiceReadRepository = voiceReadRepository;
    }

    public async Task<int> CreateAutoVoiceover(CreateAutoVoiceoverDTO dto, CancellationToken ct)
    {

        var text = await _textReadRepository.GetById(dto.TextId, ct);
        if (text is null)
        {
            throw new Exception($"Text with id: {dto.TextId} is not exist.");
        }

        var voice = await _voiceReadRepository.GetById(dto.VoiceId, ct);
        if (voice is null)
        {
            throw new Exception($"Voice with id: {dto.VoiceId} is not exist.");
        }

        // Create voiceover
        var textToSpeach = _ttsChooser(voice.ArtistModel.Source);
        var ttsDTO = new GetAudioByTextDTO(text.Text, voice.ArtistModel.Name, voice.Speed, voice.Pitch);

        var voiceoverAudioData = await textToSpeach.GetAudioByText(ttsDTO);

        if (voiceoverAudioData is null)
        {
            throw new Exception($"voiceoverAudioData is null.");
        }

        // Add file
        var URI = Guid.NewGuid().ToString();
        var filePath = Path.Combine(_acapellasPath, $"{URI}.wav");
        File.WriteAllBytes(filePath, voiceoverAudioData);

        // Place it in DB
        var durationMilliseconds = await AudioHelper.GetAudioDurationAsync(voiceoverAudioData);
        var model = AcapellaModel.Create(dto.TextId, dto.VoiceId, durationMilliseconds, 0, 0, URI); // TODO: change 0 with external service of mixering (add pause befour and after voiceover)

        var id = await _writeRepository.Create(model, ct);

        return id;
    }
}

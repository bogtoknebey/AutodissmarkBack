using Autodissmark.Application.Voiceover.ManualVoiceover.DTO;
using Autodissmark.ApplicationDataAccess.Repositories.ReadRepositories.Contracts;
using Autodissmark.ApplicationDataAccess.Repositories.WriteRepositories.Contracts;
using Autodissmark.Core.Helpers;
using Autodissmark.Domain.ApplicationModels;
using Autodissmark.Domain.Options;
using Microsoft.Extensions.Options;

namespace Autodissmark.Application.Voiceover.ManualVoiceover;

public class ManualVoiceoverLogic : IManualVoiceoverLogic
{
    private readonly string _acapellasPath;
    private readonly IAcapellaWriteRepository _writeRepository;
    private readonly ITextReadRepository _textReadRepository;

    public ManualVoiceoverLogic(
        IOptions<FilePathOptions> filePathOptions,
        IAcapellaWriteRepository writeRepository,
        ITextReadRepository textReadRepository)
    {
        _writeRepository = writeRepository;
        _acapellasPath = filePathOptions.Value.AcapellasFolderPath;
        _textReadRepository = textReadRepository;
    }

    public async Task<int> CreateManualVoiceover(CreateManualVoiceoverDTO dto, CancellationToken ct)
    {
        var text = await _textReadRepository.GetById(dto.TextId, ct);
        if (text is null)
        {
            throw new Exception($"Text with id: {dto.TextId} is not exist.");
        }

        // Add file
        var URI = Guid.NewGuid().ToString();
        var fileExtension = Path.GetExtension(dto.AudioData.FileName);

        if (fileExtension == "")
        {
            fileExtension = ".wav";
        }

        var filePath = Path.Combine(_acapellasPath, $"{URI}{fileExtension}");

        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await dto.AudioData.CopyToAsync(stream);
        }

        // Place it in DB
        var durationMilliseconds = await AudioHelper.GetAudioDurationAsync(dto.AudioData);
        var model = AcapellaModel.Create(dto.TextId, null, durationMilliseconds, 0, 0, URI);

        var id = await _writeRepository.Create(model, ct);

        return id;
    }
}

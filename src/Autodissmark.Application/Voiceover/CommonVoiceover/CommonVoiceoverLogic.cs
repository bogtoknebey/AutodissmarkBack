using Autodissmark.Application.Voiceover.CommonVoiceover.DTO;
using Autodissmark.ApplicationDataAccess.Repositories.ReadRepositories.Contracts;
using Autodissmark.ApplicationDataAccess.Repositories.WriteRepositories.Contracts;
using Autodissmark.Core.FileService.Contracts;
using Autodissmark.Domain.Options;
using Microsoft.Extensions.Options;

namespace Autodissmark.Application.Voiceover.CommonVoiceover;

public class CommonVoiceoverLogic : ICommonVoiceoverLogic
{
    private readonly IFileService _readFileService;
    private readonly IAcapellaReadRepository _readRepository;
    private readonly IAcapellaWriteRepository _writeRepository;
    private readonly string _acapellasPath;

    public CommonVoiceoverLogic(
        IFileService readFileService,
        IAcapellaReadRepository readRepository,
        IAcapellaWriteRepository writeRepository,
        IOptions<FilePathOptions> filePathOptions)
    {
        _readFileService = readFileService;
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _acapellasPath = filePathOptions.Value.AcapellasFolderPath;
    }

    public async Task<GetVoiceoverDTO> GetVoiceoverById(int id, CancellationToken ct)
    {
        var acapellaModel = await _readRepository.GetById(id, ct);
        if (acapellaModel is null)
        {
            throw new Exception($"Acapella with id: {id} is not exist.");
        }

        var fileData = await _readFileService.ReadFileAsync(_acapellasPath, acapellaModel.URI, ct);

        if (fileData is null)
        {
            throw new Exception($"File with URI:{acapellaModel.URI} is not exist.");
        }

        var dto = new GetVoiceoverDTO(acapellaModel.Id, fileData);
        return dto;
    }

    public async Task<ICollection<GetVoiceoverDTO>> GetVoiceoversByTextId(int textId, CancellationToken ct)
    {
        var models = await _readRepository.GetByTextId(textId, ct);
        var dtos = new List<GetVoiceoverDTO>();

        foreach (var model in models)
        {
            var fileData = await _readFileService.ReadFileAsync(_acapellasPath, model.URI, ct);
            var dto = new GetVoiceoverDTO(model.Id, fileData);
            dtos.Add(dto);
        }

        return dtos;
    }

    public async Task DeleteVoiceover(int id, CancellationToken ct)
    {
        await _writeRepository.Delete(id, ct);
    }
}

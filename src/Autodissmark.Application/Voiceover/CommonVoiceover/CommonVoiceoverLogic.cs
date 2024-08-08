using Autodissmark.Application.Voiceover.CommonVoiceover.DTO;
using Autodissmark.ApplicationDataAccess.Repositories.ReadRepositories.Contracts;
using Autodissmark.ApplicationDataAccess.Repositories.WriteRepositories.Contracts;
using Autodissmark.Core.FileService.Contracts;
using Autodissmark.Domain.Options;
using Microsoft.Extensions.Options;

namespace Autodissmark.Application.Voiceover.CommonVoiceover;

public class CommonVoiceoverLogic : ICommonVoiceoverLogic
{
    private readonly IFileService _fileService;
    private readonly IAcapellaReadRepository _acapellaReadRepository;
    private readonly IAcapellaWriteRepository _acapellaWriteRepository;
    private readonly string _acapellasPath;

    public CommonVoiceoverLogic(
        IFileService readFileService,
        IAcapellaReadRepository acapellaReadRepository,
        IAcapellaWriteRepository acapellaWriteRepository,
        IOptions<FilePathOptions> filePathOptions)
    {
        _fileService = readFileService;
        _acapellaReadRepository = acapellaReadRepository;
        _acapellaWriteRepository = acapellaWriteRepository;
        _acapellasPath = filePathOptions.Value.AcapellasFolderPath;
    }

    public async Task<GetVoiceoverDTO> GetVoiceoverById(int id, CancellationToken ct)
    {
        var acapellaModel = await _acapellaReadRepository.GetById(id, ct);
        if (acapellaModel is null)
        {
            throw new Exception($"Acapella with id: {id} is not exist.");
        }

        var fileData = await _fileService.ReadFileAsync(_acapellasPath, acapellaModel.URI, ct);

        if (fileData is null)
        {
            throw new Exception($"File with URI:{acapellaModel.URI} is not exist.");
        }

        var dto = new GetVoiceoverDTO(acapellaModel.Id, fileData);
        return dto;
    }

    public async Task<ICollection<GetVoiceoverDTO>> GetAllVoiceovers(int textId, CancellationToken ct)
    {
        var models = await _acapellaReadRepository.GetAllByTextId(textId, ct);
        var dtos = new List<GetVoiceoverDTO>();

        foreach (var model in models)
        {
            var fileData = await _fileService.ReadFileAsync(_acapellasPath, model.URI, ct);
            var dto = new GetVoiceoverDTO(model.Id, fileData);
            dtos.Add(dto);
        }

        return dtos;
    }

    public async Task<ICollection<GetVoiceoverDTO>> GetVoiceoversPage(int textId, int pageSize, int pageNumber, CancellationToken ct)
    {
        var models = await _acapellaReadRepository.GetPageByTextId(textId, pageSize, pageNumber, ct);
        var dtos = new List<GetVoiceoverDTO>();

        foreach (var model in models)
        {
            var fileData = await _fileService.ReadFileAsync(_acapellasPath, model.URI, ct);
            var dto = new GetVoiceoverDTO(model.Id, fileData);
            dtos.Add(dto);
        }

        return dtos;
    }

    public async Task DeleteVoiceover(int id, CancellationToken ct)
    {
        var acapellaModel = await _acapellaReadRepository.GetById(id, ct);

        if (acapellaModel is null)
        {
            throw new Exception($"Acapella with id: {id} is not exist.");
        }

        _fileService.DeleteFileIfExist(_acapellasPath, acapellaModel.URI);

        await _acapellaWriteRepository.Delete(id, ct);
    }
}

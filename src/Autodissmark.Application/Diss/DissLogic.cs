using Autodissmark.Application.Diss.DTO;
using Autodissmark.ApplicationDataAccess.Repositories.ReadRepositories.Contracts;
using Autodissmark.ApplicationDataAccess.Repositories.WriteRepositories.Contracts;
using Autodissmark.AudioMixer.Mixer;
using Autodissmark.AudioMixer.Mixer.DTO;
using Autodissmark.Core.FileService.Contracts;
using Autodissmark.Domain.ApplicationModels.Diss;
using Autodissmark.Domain.Options;
using Microsoft.Extensions.Options;
using System;

namespace Autodissmark.Application.Diss;

public class DissLogic : IDissLogic
{
    private const float BeatVolume = 0.45f;
    private const float AcapellaVolume = 0.95f;
    private const string DissExtension = ".mp3"; // TODO: replace it to Mixer service (Compress method)

    private readonly string _beatsPath;
    private readonly string _acapellasPath;
    private readonly string _dissesPath;

    private readonly IFileService _fileService;
    private readonly IMixer _mixer;
    private readonly IAcapellaReadRepository _acapellaReadRepository;
    private readonly IBeatReadRepository _beatReadRepository;
    private readonly IDissReadRepository _dissReadRepository;
    private readonly IDissWriteRepository _dissWriteRepository;


    public DissLogic(
        IOptions<FilePathOptions> filePathOptions,
        IFileService fileService,
        IMixer mixer,
        IAcapellaReadRepository acapellaReadRepository,
        IBeatReadRepository beatReadRepository,
        IDissReadRepository dissReadRepository,
        IDissWriteRepository dissWriteRepository
    )
    {
        _beatsPath = filePathOptions.Value.BeatsFolderPath;
        _acapellasPath = filePathOptions.Value.AcapellasFolderPath;
        _dissesPath = filePathOptions.Value.DissesFolderPath;

        _fileService = fileService;
        _mixer = mixer;
        _acapellaReadRepository = acapellaReadRepository;
        _beatReadRepository = beatReadRepository;
        _dissReadRepository = dissReadRepository;
        _dissWriteRepository = dissWriteRepository;
    }

    public async Task<int> CreateDiss(CreateDissDTO dto, CancellationToken ct)
    {
        // Beat
        var beat = await _beatReadRepository.GetById(dto.BeatId, ct);

        if (beat is null)
        {
            throw new Exception($"Beat with id: {dto.BeatId} is not exist.");
        }

        var beatFilePath = _fileService.GetFilePath(_beatsPath, beat.URI);
        BaseMixFileDTO beatMixFileDTO = new BaseMixFileDTO(beatFilePath, BeatVolume);

        // Acapella
        var acapella = await _acapellaReadRepository.GetById(dto.AcapellaId, ct);

        if (acapella is null)
        {
            throw new Exception($"Acapella with id: {dto.AcapellaId} is not exist.");
        }

        var acapellaFilePath = _fileService.GetFilePath(_acapellasPath, acapella.URI);
        MixFileDTO acapellaMixFileDTO = new MixFileDTO(acapellaFilePath, AcapellaVolume);

        // Form diss file
        var dissURI = Guid.NewGuid().ToString();
        var dissFilePath = Path.Combine(_dissesPath, $"{dissURI}{DissExtension}");

        await _mixer.MixFiles(beatMixFileDTO, acapellaMixFileDTO, dissFilePath, ct);

        // Save diss
        var dissAcapellas = new List<DissAcapellaModel>() { DissAcapellaModel.Create(dto.AcapellaId, 0) };
        var dissModel = DissModel.Create(
            dto.BeatId,
            dissURI,
            dto.Target,
            dissAcapellas
        );

        var dissId = await _dissWriteRepository.Create(dissModel, ct);

        return dissId;
    }

    public async Task<GetDissDTO> GetDiss(int id, CancellationToken ct)
    {
        var dissModel = await _dissReadRepository.GetById(id, ct);
        if (dissModel is null)
        {
            throw new Exception($"Diss with id: {id} is not exist.");
        }

        var fileData = await _fileService.ReadFileAsync(_dissesPath, dissModel.URI, ct);

        if (fileData is null)
        {
            throw new Exception($"File with URI:{dissModel.URI} is not exist.");
        }

        var dto = new GetDissDTO(dissModel.Id, fileData);
        return dto;
    }

    public async Task DeleteDiss(int id, CancellationToken ct)
    {
        var dissModel = await _dissReadRepository.GetById(id, ct);

        if (dissModel is null)
        {
            throw new Exception($"Diss with id: {id} is not exist.");
        }

        _fileService.DeleteFileIfExist(_dissesPath, dissModel.URI);

        await _dissWriteRepository.Delete(id, ct);
    }
}

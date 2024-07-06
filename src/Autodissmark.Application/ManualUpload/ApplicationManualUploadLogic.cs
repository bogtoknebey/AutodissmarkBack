using Autodissmark.ApplicationDataAccess.Repositories.WriteRepositories.Contracts;
using Autodissmark.Domain.ApplicationModels;

namespace Autodissmark.Application.ManualUpload;

public class ApplicationManualUploadLogic : IApplicationManualUploadLogic
{
    private readonly IBeatWriteRepository _beatWriteRepository;

    public ApplicationManualUploadLogic(IBeatWriteRepository beatWriteRepository)
    {
        _beatWriteRepository = beatWriteRepository;
    }

    public async Task<int> UploadBeats(string path)
    {
        int count = 0;
        var filePaths = Directory.GetFiles(path);
        foreach (var filePath in filePaths)
        {
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
            var fileExtension = Path.GetExtension(filePath);

            if (Guid.TryParse(fileNameWithoutExtension, out Guid guid))
            {
                continue;
            }

            var URI = Guid.NewGuid().ToString();
            var beat = BeatModel.Create(URI);
            await _beatWriteRepository.Create(beat);

            // Rename file
            string directoryPath = Path.GetDirectoryName(filePath);
            var newFileName = $"{URI}{fileExtension}";
            string newFilePath = Path.Combine(directoryPath, newFileName);
            File.Move(filePath, newFilePath);

            count++;
        }

        return count;
    }
}

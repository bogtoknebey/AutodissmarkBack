using Autodissmark.Core.FileService.Contracts;

namespace Autodissmark.Core.FileService;

public class FileService : IFileService
{
    public async Task<byte[]> ReadFileAsync(string path, string URI, CancellationToken ct)
    {
        string filePath = GetFilePath(path, URI);

        if (filePath is null)
        {
            return null;
        }

        return await File.ReadAllBytesAsync(filePath, ct);
    }

    public string GetFilePath(string path, string URI)
    {
        string[] files = Directory.GetFiles(path, $"{URI}.*");

        if (files.Length == 0)
        {
            return null;
        }

        if (files.Length > 1)
        {
            throw new Exception($"More than one file have the same URI. Path:{path}, URI:{URI}");
        }

        return files[0];
    }

    public void DeleteFileIfExist(string path, string URI)
    {
        var filePath = GetFilePath(path, URI);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }
}

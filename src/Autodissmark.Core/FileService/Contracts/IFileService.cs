namespace Autodissmark.Core.FileService.Contracts;

public interface IFileService
{
    Task<byte[]> ReadFileAsync(string path, string URI, CancellationToken ct);
    string GetFilePath(string path, string URI);
    void DeleteFileIfExist(string path, string URI);
}

using System.Reflection.Metadata;

namespace Autodissmark.Application.ManualUpload;

public interface IApplicationManualUploadLogic
{
    Task<int> UploadBeats(string path);
}

namespace Autodissmark.TextProcessor.ManuallyUpload;

public interface ITextProcessorManuallyUploadLogic
{
    Task<int> UploadDictionaries(string path);
}

using Autodissmark.Domain.TextProcessorModels;
using Autodissmark.TextProcessorDataAccess.Repositories.WriteRepositories.Contracts;

namespace Autodissmark.TextProcessor.ManuallyUpload;

public class TextProcessorManuallyUploadLogic : ITextProcessorManuallyUploadLogic
{
    private readonly IDictionaryWriteRepository _dictionaryWriteRepository;
    private readonly IDictionaryWordWriteRepository _dictionaryWordWriteRepository;

    public TextProcessorManuallyUploadLogic(
        IDictionaryWordWriteRepository dictionaryWordWriteRepository, 
        IDictionaryWriteRepository dictionaryWriteRepository)
    {
        _dictionaryWordWriteRepository = dictionaryWordWriteRepository;
        _dictionaryWriteRepository = dictionaryWriteRepository;
    }

    public async Task<int> UploadDictionaries(string path)
    {
        int count = 0;
        var filePaths = Directory.GetFiles(path);
        foreach (var filePath in filePaths)
        {
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
            
            if (Guid.TryParse(fileNameWithoutExtension, out Guid guid))
            {
                continue;
            }

            var dictionary = await GetDictionaryModel(filePath);

            dictionary.Id = await _dictionaryWriteRepository.Create(dictionary);
            await _dictionaryWordWriteRepository.CreateDictionaryWords(dictionary);

            // Rename file
            string directoryPath = Path.GetDirectoryName(filePath);
            var newFileName = $"{Guid.NewGuid()}.txt";
            string newFilePath = Path.Combine(directoryPath, newFileName);
            File.Move(filePath, newFilePath);

            count++;
        }

        return count;
    }

    private async Task<DictionaryModel> GetDictionaryModel(string filePath)
    {
        string[] lines = await File.ReadAllLinesAsync(filePath);
        List<string> words = new List<string>(lines);

        DictionaryModel dictionaryModel = DictionaryModel.Create(
            Path.GetFileNameWithoutExtension(filePath), 
            words
        );

        return dictionaryModel;
    }
}

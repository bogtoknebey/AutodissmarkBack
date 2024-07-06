using Autodissmark.Domain.TextProcessorModels;

namespace Autodissmark.TextProcessorDataAccess.Repositories.WriteRepositories.Contracts;

public interface IDictionaryWordWriteRepository
{
    Task CreateDictionaryWords(DictionaryModel model);
}

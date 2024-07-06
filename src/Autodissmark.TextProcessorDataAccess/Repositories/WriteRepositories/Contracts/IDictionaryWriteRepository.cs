using Autodissmark.Domain.TextProcessorModels;

namespace Autodissmark.TextProcessorDataAccess.Repositories.WriteRepositories.Contracts;

public interface IDictionaryWriteRepository
{
    Task<int> Create(DictionaryModel model, CancellationToken ct = default);
}

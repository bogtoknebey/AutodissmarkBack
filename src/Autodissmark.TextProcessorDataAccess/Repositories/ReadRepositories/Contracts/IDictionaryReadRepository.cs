using Autodissmark.Domain.TextProcessorModels;

namespace Autodissmark.TextProcessorDataAccess.Repositories.ReadRepositories.Contracts;

public interface IDictionaryReadRepository
{
    Task<DictionaryModel> GetById(int id, CancellationToken ct = default);
    Task<int> GetFirstIdByName(string name, CancellationToken ct = default);
}

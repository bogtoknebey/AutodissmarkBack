using Autodissmark.Domain.ApplicationModels;

namespace Autodissmark.ApplicationDataAccess.Repositories.WriteRepositories.Contracts;

public interface ITextWriteRepository
{
    Task<int> Create(TextModel model, CancellationToken ct = default);
    Task Update(int id, TextModel model, CancellationToken ct = default);
    Task Delete(int id, CancellationToken ct = default);
}



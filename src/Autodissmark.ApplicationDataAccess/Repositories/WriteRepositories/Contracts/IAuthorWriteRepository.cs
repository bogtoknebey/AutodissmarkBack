using Autodissmark.Domain.ApplicationModels;

namespace Autodissmark.ApplicationDataAccess.Repositories.WriteRepositories.Contracts;

public interface IAuthorWriteRepository
{
    Task<int> Create(AuthorModel model, CancellationToken ct = default);
}


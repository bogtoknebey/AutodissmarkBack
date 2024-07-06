using Autodissmark.Domain.ApplicationModels;

namespace Autodissmark.ApplicationDataAccess.Repositories.ReadRepositories.Contracts;

public interface IAuthorReadRepository
{
    Task<AuthorModel> GetById(int id, CancellationToken ct = default);
    Task<AuthorModel> GetByEmail(string email, CancellationToken ct = default);
    Task<int> GetAuthorsCount(CancellationToken ct = default);
    Task<ICollection<AuthorModel>> GetAllAuthors(CancellationToken ct = default);
}

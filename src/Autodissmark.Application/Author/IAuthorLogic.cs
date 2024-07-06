using Autodissmark.Application.Author.DTO;
using Autodissmark.Domain.ApplicationModels;

namespace Autodissmark.Application.Author;

public interface IAuthorLogic
{
    Task<int> CreateAuthor(CreateAuthorInputDTO dto, CancellationToken ct);
    Task<AuthorModel> GetAuthorById(int id, CancellationToken ct);
    Task<int> GetAuthorsCount(CancellationToken ct);
    Task<ICollection<AuthorModel>> GetAllAuthors(CancellationToken ct);
}

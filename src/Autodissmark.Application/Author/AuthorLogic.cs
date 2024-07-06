using Autodissmark.Application.Author.DTO;
using Autodissmark.ApplicationDataAccess.Repositories.ReadRepositories.Contracts;
using Autodissmark.ApplicationDataAccess.Repositories.WriteRepositories.Contracts;
using Autodissmark.Domain.ApplicationModels;

namespace Autodissmark.Application.Author;

public class AuthorLogic : IAuthorLogic
{
    private readonly IAuthorReadRepository _readRepository;
    private readonly IAuthorWriteRepository _writeRepository;

    public AuthorLogic(IAuthorReadRepository readRepository, IAuthorWriteRepository writeRepository)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
    }

    public Task<AuthorModel> GetAuthorById(int id, CancellationToken ct)
    {
        return _readRepository.GetById(id);
    }

    public Task<int> GetAuthorsCount(CancellationToken ct)
    {
        return _readRepository.GetAuthorsCount();
    }

    public Task<int> CreateAuthor(CreateAuthorInputDTO dto, CancellationToken ct)
    {
        var authorModel = AuthorModel.Create
        (
            dto.Name,
            dto.Email,
            dto.Password,
            dto.Role
        );

        return _writeRepository.Create(authorModel);
    }

    public async Task<ICollection<AuthorModel>> GetAllAuthors(CancellationToken ct)
    {
        return await _readRepository.GetAllAuthors(ct);
    }
}

using Autodissmark.Domain.ApplicationModels;

namespace Autodissmark.ApplicationDataAccess.Repositories.ReadRepositories.Contracts;

public interface ITextReadRepository
{
    Task<TextModel> GetById(int id, CancellationToken ct = default);
    Task<int> GetTextsCount(int authorId, CancellationToken ct = default);
    Task<ICollection<TextModel>> GetRandomTexts(int authorId, int textsCount, CancellationToken ct = default);
    Task<ICollection<TextModel>> GetAllTexts(int authorId, CancellationToken ct = default);
    Task<int> GetAuthorId(int id, CancellationToken ct = default);
}
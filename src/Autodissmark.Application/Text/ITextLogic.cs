using Autodissmark.Application.Text.DTO;
using Autodissmark.Domain.ApplicationModels;

namespace Autodissmark.Application.Text;

public interface ITextLogic
{
    Task<int> CreateText(CreateTextInputDTO dto, CancellationToken ct);
    Task<TextModel> GetTextById(int id, CancellationToken ct);
    Task<ICollection<TextModel>> GetTextsPage(int authorId, int pageSize, int pageNumber, CancellationToken ct);
    Task<ICollection<TextModel>> GetAllTexts(int authorId, CancellationToken ct);
    Task<int> GetTextsCount(int authorId, CancellationToken ct);
    Task<ICollection<TextModel>> GetRandomTexts(int authorId, int textsCount, CancellationToken ct);
    Task UpdateText(UpdateTextInputDTO dto, CancellationToken ct);
    Task DeleteText(int textId, CancellationToken ct);
}
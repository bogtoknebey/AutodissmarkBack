using Autodissmark.Application.Text.DTO;
using Autodissmark.ApplicationDataAccess.Repositories.ReadRepositories.Contracts;
using Autodissmark.ApplicationDataAccess.Repositories.WriteRepositories.Contracts;
using Autodissmark.Domain.ApplicationModels;

namespace Autodissmark.Application.Text;

public class TextLogic : ITextLogic
{
    private readonly ITextReadRepository _readRepository;
    private readonly ITextWriteRepository _writeRepository;

    public TextLogic(ITextReadRepository readRepository, ITextWriteRepository writeRepository)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
    }

    public async Task<int> CreateText(CreateTextInputDTO dto, CancellationToken ct)
    {
        var textModel = TextModel.Create
        (
            dto.AuthorId,
            dto.Text,
            dto.Title,
            DateTime.UtcNow
        );

        return await _writeRepository.Create(textModel, ct);
    }

    public async Task<TextModel> GetTextById(int id, CancellationToken ct)
    {
        return await _readRepository.GetById(id, ct);
    }

    public async Task<ICollection<TextModel>> GetTextsPage(int authorId, int pageSize, int pageNumber, CancellationToken ct)
    {
        return await _readRepository.GetTextsPage(authorId, pageSize, pageNumber, ct);
    }

    public async Task<ICollection<TextModel>> GetAllTexts(int authorId, CancellationToken ct)
    {
        return await _readRepository.GetAllTexts(authorId, ct);
    }

    public async Task<int> GetTextsCount(int authorId, CancellationToken ct)
    {
        return await _readRepository.GetTextsCount(authorId, ct);
    }

    public async Task<ICollection<TextModel>> GetRandomTexts(int authorId, int textsCount, CancellationToken ct = default)
    {
        return await _readRepository.GetRandomTexts(authorId, textsCount, ct);
    }

    public async Task UpdateText(UpdateTextInputDTO dto, CancellationToken ct)
    {
        var textModel = TextModel.Create
        (
            dto.AuthorId,
            dto.Text,
            dto.Title,
            DateTime.UtcNow
        );

        await _writeRepository.Update(dto.Id, textModel, ct);
    }

    public async Task DeleteText(int textId, CancellationToken ct)
    {
        await _writeRepository.Delete(textId, ct);
    }
}


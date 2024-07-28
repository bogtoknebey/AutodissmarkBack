using Autodissmark.ApplicationDataAccess.Repositories.ReadRepositories.Contracts;
using Autodissmark.Domain.ApplicationModels;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Autodissmark.ApplicationDataAccess.Repositories.ReadRepositories;

public class TextReadRepository : ITextReadRepository
{
    private readonly ApplicationDataContext _context;
    private readonly IMapper _mapper;

    public TextReadRepository(ApplicationDataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<TextModel> GetById(int id, CancellationToken ct = default)
    {
        var entity = await _context.Texts.FirstOrDefaultAsync(e => e.Id == id, ct);
        return _mapper.Map<TextModel>(entity);
    }

    public async Task<int> GetTextsCount(int authorId, CancellationToken ct = default)
    {
        return await _context.Texts
            .Where(t => t.AuthorEntityId == authorId)
            .CountAsync(ct);
    }

    public async Task<ICollection<TextModel>> GetTextsPage(int authorId, int pageSize, int pageNumber, CancellationToken ct = default)
    {
        int skipAmount = (pageNumber - 1) * pageSize;

        var entities = await _context.Texts
            .Where(t => t.AuthorEntityId == authorId)
            .Skip(skipAmount)
            .Take(pageSize)
            .ToListAsync(ct);

        return _mapper.Map<ICollection<TextModel>>(entities);
    }

    public async Task<ICollection<TextModel>> GetRandomTexts(int authorId, int textsCount, CancellationToken ct = default)
    {
        var sql = $"SELECT TOP({textsCount}) * " +
                  $"FROM Texts " +
                  $"Where AuthorEntityId = {authorId} " +
                  $"ORDER BY NEWID()";

        var entities = await _context.Texts
            .FromSqlRaw(sql)
            .ToListAsync(ct);

        return _mapper.Map<ICollection<TextModel>>(entities);
    }

    public async Task<ICollection<TextModel>> GetAllTexts(int authorId, CancellationToken ct = default)
    {
        var entities = await _context.Texts
            .Where(t => t.AuthorEntityId == authorId)
            .ToListAsync(ct);

        return _mapper.Map<ICollection<TextModel>>(entities);
    }

    public async Task<int> GetAuthorId(int id, CancellationToken ct = default)
    {
        var entity = await _context.Texts.FirstOrDefaultAsync(e => e.Id == id, ct);
        return entity.AuthorEntityId;
    }
}


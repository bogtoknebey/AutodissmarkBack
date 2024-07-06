using Autodissmark.ApplicationDataAccess.Entities;
using Autodissmark.ApplicationDataAccess.Repositories.WriteRepositories.Contracts;
using Autodissmark.Domain.ApplicationModels;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Autodissmark.ApplicationDataAccess.Repositories.WriteRepositories;

public class TextWriteRepository : ITextWriteRepository
{
    private readonly ApplicationDataContext _context;
    private readonly IMapper _mapper;

    public TextWriteRepository(ApplicationDataContext dataContext, IMapper mapper)
    {
        _context = dataContext;
        _mapper = mapper;
    }

    public async Task<int> Create(TextModel model, CancellationToken ct = default)
    {
        var entity = _mapper.Map<TextEntity>(model);
        await _context.Texts.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity.Id;
    }

    public async Task Update(int id, TextModel model, CancellationToken ct = default)
    {
        var entity = await _context.Texts.FindAsync(id, ct);

        if (entity == null)
        {
            throw new ArgumentException($"Text with id {id} not found");
        }

        entity.Text = model.Text;
        entity.Title = model.Title;

        await _context.SaveChangesAsync(ct);
    }

    public async Task Delete(int id, CancellationToken ct = default)
    {
        var entity = await _context.Texts.FindAsync(id, ct);

        if (entity == null)
        {
            throw new ArgumentException($"Text with id {id} not found");
        }

        _context.Texts.Remove(entity);
        await _context.SaveChangesAsync(ct);
    }
}


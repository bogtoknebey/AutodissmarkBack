using Autodissmark.ApplicationDataAccess.Repositories.ReadRepositories.Contracts;
using Autodissmark.Domain.ApplicationModels;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Autodissmark.ApplicationDataAccess.Repositories.ReadRepositories;

public class AuthorReadRepository : IAuthorReadRepository
{
    private readonly ApplicationDataContext _context;
    private readonly IMapper _mapper;

    public AuthorReadRepository(ApplicationDataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<AuthorModel> GetById(int id, CancellationToken ct = default)
    {
        var entity = await _context.Authors.FirstOrDefaultAsync(d => d.Id == id, ct);
        return _mapper.Map<AuthorModel>(entity);
    }

    public async Task<AuthorModel> GetByEmail(string email, CancellationToken ct = default)
    {
        var entity = await _context.Authors.FirstOrDefaultAsync(d => d.Email == email, ct);
        return _mapper.Map<AuthorModel>(entity);
    }

    public async Task<int> GetAuthorsCount(CancellationToken ct = default)
    {
        return await _context.Authors.CountAsync(ct);
    }

    public async Task<ICollection<AuthorModel>> GetAllAuthors(CancellationToken ct = default)
    {
        var entities = await _context.Authors.ToListAsync(ct);
        var models = entities.Select(_mapper.Map<AuthorModel>).ToList();
        return models;
    }
}

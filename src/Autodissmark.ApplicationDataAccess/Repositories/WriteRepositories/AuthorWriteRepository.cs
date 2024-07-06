using Autodissmark.ApplicationDataAccess.Entities;
using Autodissmark.ApplicationDataAccess.Repositories.WriteRepositories.Contracts;
using Autodissmark.Domain.ApplicationModels;
using AutoMapper;

namespace Autodissmark.ApplicationDataAccess.Repositories.WriteRepositories;

public class AuthorWriteRepository : IAuthorWriteRepository
{
    private readonly ApplicationDataContext _context;
    private readonly IMapper _mapper;

    public AuthorWriteRepository(ApplicationDataContext dataContext, IMapper mapper)
    {
        _context = dataContext;
        _mapper = mapper;
    }

    public async Task<int> Create(AuthorModel model, CancellationToken ct = default)
    {
        var entity = _mapper.Map<AuthorEntity>(model);
        await _context.Authors.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity.Id;
    }
}

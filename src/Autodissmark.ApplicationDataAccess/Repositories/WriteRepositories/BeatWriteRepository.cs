using Autodissmark.ApplicationDataAccess.Entities;
using Autodissmark.ApplicationDataAccess.Repositories.WriteRepositories.Contracts;
using Autodissmark.Domain.ApplicationModels;
using AutoMapper;

namespace Autodissmark.ApplicationDataAccess.Repositories.WriteRepositories;

public class BeatWriteRepository : IBeatWriteRepository
{
    private readonly ApplicationDataContext _context;
    private readonly IMapper _mapper;

    public BeatWriteRepository(ApplicationDataContext dataContext, IMapper mapper)
    {
        _context = dataContext;
        _mapper = mapper;
    }

    public async Task<int> Create(BeatModel model, CancellationToken ct = default)
    {
        var entity = _mapper.Map<BeatEntity>(model);
        await _context.Beats.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity.Id;
    }
}

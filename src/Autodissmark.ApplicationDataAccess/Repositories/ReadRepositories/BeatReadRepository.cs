using Autodissmark.ApplicationDataAccess.Repositories.ReadRepositories.Contracts;
using Autodissmark.Domain.ApplicationModels;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Autodissmark.ApplicationDataAccess.Repositories.ReadRepositories;

public class BeatReadRepository : IBeatReadRepository
{
    private readonly ApplicationDataContext _context;
    private readonly IMapper _mapper;

    public BeatReadRepository(ApplicationDataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<BeatModel> GetById(int id, CancellationToken ct = default)
    {
        var entity = await _context.Beats.FirstOrDefaultAsync(e => e.Id == id, ct);
        return _mapper.Map<BeatModel>(entity);
    }
}

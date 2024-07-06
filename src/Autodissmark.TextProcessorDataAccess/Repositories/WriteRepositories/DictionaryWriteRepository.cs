using Autodissmark.Domain.TextProcessorModels;
using Autodissmark.TextProcessorDataAccess.Entities;
using Autodissmark.TextProcessorDataAccess.Repositories.WriteRepositories.Contracts;
using AutoMapper;

namespace Autodissmark.TextProcessorDataAccess.Repositories.WriteRepositories;

public class DictionaryWriteRepository : IDictionaryWriteRepository
{
    private readonly TextProcessorDataContext _context;
    private readonly IMapper _mapper;

    public DictionaryWriteRepository(TextProcessorDataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<int> Create(DictionaryModel model, CancellationToken ct = default)
    {
        var entity = _mapper.Map<DictionaryEntity>(model);
        await _context.Dictionaries.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity.Id;
    }
}

using Autodissmark.Domain.TextProcessorModels;
using Autodissmark.TextProcessorDataAccess.Entities;
using Autodissmark.TextProcessorDataAccess.Repositories.WriteRepositories.Contracts;
using AutoMapper;

namespace Autodissmark.TextProcessorDataAccess.Repositories.WriteRepositories;

public class DictionaryWordWriteRepository : IDictionaryWordWriteRepository
{
    private readonly TextProcessorDataContext _context;
    private readonly IMapper _mapper;

    public DictionaryWordWriteRepository(TextProcessorDataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task CreateDictionaryWords(DictionaryModel model)
    {
        var entities = _mapper.Map<List<DictionaryWordEntity>>(model);

        await _context.DictionaryWords.AddRangeAsync(entities);
        await _context.SaveChangesAsync();
    }
}

using Autodissmark.Domain.TextProcessorModels;
using Autodissmark.TextProcessorDataAccess.Entities;
using AutoMapper;

namespace Autodissmark.TextProcessorDataAccess.MappingProfile;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<DictionaryModel, DictionaryEntity>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
        CreateMap<DictionaryEntity, DictionaryModel>()
            .ForMember(dest => dest.Words, opt => opt.Ignore());

        CreateMap<DictionaryModel, List<DictionaryWordEntity>>()
            .ConvertUsing((src, dest, context) =>
            {
                var dictionaryEntityId = src.Id;

                var entities = new List<DictionaryWordEntity>();

                foreach (var word in src.Words)
                {
                    entities.Add(new DictionaryWordEntity
                    {
                        DictionaryEntityId = dictionaryEntityId,
                        Word = word
                    });
                }

                return entities;
            });
    }
}

using Autodissmark.ApplicationDataAccess.Entities;
using Autodissmark.Domain.ApplicationModels;
using Autodissmark.Domain.ApplicationModels.Diss;
using AutoMapper;

namespace Autodissmark.ApplicationDataAccess.MappingProfile;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<AuthorEntity, AuthorModel>().ReverseMap();
        CreateMap<BeatEntity, BeatModel>().ReverseMap();
        CreateMap<TextEntity, TextModel>().ReverseMap();

        CreateMap<ICollection<TextEntity>, ICollection<TextModel>>()
            .ConvertUsing((src, dest, context) =>
            {
                var mapper = context.Mapper;
                var textModels = src.Select(e => mapper.Map<TextModel>(e)).ToList();
                return textModels;
            });

        CreateMap<AcapellaEntity, AcapellaModel>().ReverseMap();

        CreateMap<VoiceEntity, VoiceModel>()
            .ForMember(dest => dest.ArtistId, opt => opt.MapFrom(src => src.ArtistEntityId))
            .ForMember(dest => dest.ArtistModel, opt => opt.MapFrom(src => src.ArtistEntity));

        CreateMap<ArtistEntity, ArtistModel>();

        CreateMap<DissAcapellaEntity, DissAcapellaModel>()
            .ForMember(dest => dest.DissId, opt => opt.MapFrom(src => src.DissEntityId))
            .ForMember(dest => dest.AcapellaId, opt => opt.MapFrom(src => src.AcapellaEntityId));
        CreateMap<DissAcapellaModel, DissAcapellaEntity>()
            .ForMember(dest => dest.DissEntityId, opt => opt.MapFrom(src => src.DissId))
            .ForMember(dest => dest.AcapellaEntityId, opt => opt.MapFrom(src => src.AcapellaId));

        CreateMap<DissEntity, DissModel>()
            .ForMember(dest => dest.BeatId, opt => opt.MapFrom(src => src.BeatEntityId))
            .ForMember(dest => dest.DissAcapellas, opt => opt.MapFrom(src => src.DissAcapellaEntities));
        CreateMap<DissModel, DissEntity>()
            .ForMember(dest => dest.BeatEntityId, opt => opt.MapFrom(src => src.BeatId))
            .ForMember(dest => dest.DissAcapellaEntities, opt => opt.MapFrom(src => src.DissAcapellas));
    }
}

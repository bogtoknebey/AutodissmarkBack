using Autodissmark.API.Requests;
using Autodissmark.API.Responses;
using Autodissmark.Application.Author.DTO;
using Autodissmark.Application.Diss.DTO;
using Autodissmark.Application.Login.DTO;
using Autodissmark.Application.Text.DTO;
using Autodissmark.Application.Voiceover.AutoVoiceover.DTO;
using Autodissmark.Application.Voiceover.CommonVoiceover.DTO;
using Autodissmark.Application.Voiceover.ManualVoiceover.DTO;
using Autodissmark.Domain.ApplicationModels;
using AutoMapper;

namespace Autodissmark.API.MappingProfile;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateAuthorRequest, CreateAuthorInputDTO>();
        CreateMap<CreateTextRequest, CreateTextInputDTO>();
        CreateMap<TextModel, GetRandomTextResponse>();
        CreateMap<UpdateTextRequest, UpdateTextInputDTO>();
        CreateMap<CreateManualVoiceoverRequest, CreateManualVoiceoverDTO>();
        CreateMap<GetVoiceoverDTO, GetVoiceoverResponse>();
        CreateMap<CreateAutoVoiceoverRequest, CreateAutoVoiceoverDTO>();
        CreateMap<GetDissDTO, GetDissResponse>();
        CreateMap<CreateDissRequest, CreateDissDTO>();
        CreateMap<LoginRequest, LoginInputDTO>();
    }
}

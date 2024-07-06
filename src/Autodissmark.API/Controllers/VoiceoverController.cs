using Autodissmark.API.ExceptionHandling;
using Autodissmark.API.Requests;
using Autodissmark.API.Responses.BaseResponses;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Autodissmark.API.Responses;
using Autodissmark.Application.Voiceover.CommonVoiceover;
using Autodissmark.Application.Voiceover.ManualVoiceover;
using Autodissmark.Application.Voiceover.ManualVoiceover.DTO;
using Autodissmark.Application.Voiceover.AutoVoiceover;
using Autodissmark.Application.Voiceover.AutoVoiceover.DTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Autodissmark.Domain.Enums;

namespace Autodissmark.API.Controllers;

[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Authorize(Roles = nameof(Role.Admin) + "," + nameof(Role.User))]
[ApiController]
public class VoiceoverController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IManualVoiceoverLogic _manualVoiceoverLogic;
    private readonly ICommonVoiceoverLogic _commonVoiceoverLogic;
    private readonly IAutoVoiceoverLogic _autoVoiceoverLogic;

    public VoiceoverController(
        IMapper mapper,
        IManualVoiceoverLogic manualVoiceoverLogic,
        ICommonVoiceoverLogic commonVoiceoverLogic,
        IAutoVoiceoverLogic autoVoiceoverLogic
    )
    {
        _mapper = mapper;
        _manualVoiceoverLogic = manualVoiceoverLogic;
        _commonVoiceoverLogic = commonVoiceoverLogic;
        _autoVoiceoverLogic = autoVoiceoverLogic;
    }

    [HttpPost("create-manual-voiceover")]
    public async Task<IActionResult> CreateManualVoiceover([FromForm] CreateManualVoiceoverRequest request, CancellationToken ct)
    {
        try
        {
            // TODO: setup authorId (check if authorId match with textAuthorId)
            var dto = _mapper.Map<CreateManualVoiceoverDTO>(request);
            var id = await _manualVoiceoverLogic.CreateManualVoiceover(dto, ct);
            return Ok(new SuccessResponse<int>(id));
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponse(ExceptionCodes.InternalServerError, ex.Message));
        }
    }

    [HttpPost("create-auto-voiceover")]
    public async Task<IActionResult> CreateAutoVoiceover([FromBody] CreateAutoVoiceoverRequest request, CancellationToken ct)
    {
        try
        {
            // TODO: setup authorId (check if authorId match with textAuthorId)
            var dto = _mapper.Map<CreateAutoVoiceoverDTO>(request);
            var id = await _autoVoiceoverLogic.CreateAutoVoiceover(dto, ct);
            return Ok(new SuccessResponse<int>(id));
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponse(ExceptionCodes.InternalServerError, ex.Message));
        }
    }

    [HttpGet("get-voiceover")]
    public async Task<IActionResult> GetVoiceover(int id, CancellationToken ct)
    {
        try
        {
            // TODO: setup authorId (check if authorId match with textAuthorId of acapell's entity)
            var dto = await _commonVoiceoverLogic.GetVoiceoverById(id, ct);
            var response = _mapper.Map<GetVoiceoverResponse>(dto);
            return Ok(new SuccessResponse<GetVoiceoverResponse>(response));
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponse(ExceptionCodes.InternalServerError, ex.Message));
        }
    }

    [HttpGet("get-text-voiceovers")]
    public async Task<IActionResult> GetVoiceoversByTextId(int textId, CancellationToken ct)
    {
        try 
        {
            // TODO: setup authorId (check if authorId match with textAuthorId)
            // TODO: implement pagination(receive int pageCapacity, int pageNumber)
            var dtos = await _commonVoiceoverLogic.GetVoiceoversByTextId(textId, ct);
            var responses = dtos.Select(_mapper.Map<GetVoiceoverResponse>).ToList();
            return Ok(new SuccessResponse<List<GetVoiceoverResponse>>(responses));
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponse(ExceptionCodes.InternalServerError, ex.Message));
        }
    }

    [HttpDelete("delete-voiceover")]
    public async Task<IActionResult> DeleteVoiceover([FromBody] int id, CancellationToken ct)
    {
        try
        {
            // TODO: setup authorId (check if authorId match with textAuthorId of acapell's entity)
            await _commonVoiceoverLogic.DeleteVoiceover(id, ct);
            return Ok(new SuccessResponse());
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponse(ExceptionCodes.InternalServerError, ex.Message));
        }
    }
}

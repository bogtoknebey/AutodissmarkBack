using Autodissmark.API.ExceptionHandling;
using Autodissmark.API.Requests;
using Autodissmark.API.Responses;
using Autodissmark.API.Responses.BaseResponses;
using Autodissmark.Application.Diss;
using Autodissmark.Application.Diss.DTO;
using Autodissmark.Domain.Enums;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Autodissmark.API.Controllers;

[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Authorize(Roles = nameof(Role.Admin) + "," + nameof(Role.User))]
[ApiController]
public class DissController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IDissLogic _dissLogic;

    public DissController(
        IMapper mapper,
        IDissLogic dissLogic
    )
    {
        _mapper = mapper;
        _dissLogic = dissLogic;
    }

    [HttpPost("create-diss")]
    public async Task<IActionResult> CreateDiss([FromBody] CreateDissRequest request, CancellationToken ct)
    {
        try
        {
            // TODO: setup authorId (for check permision)
            var dto = _mapper.Map<CreateDissDTO>(request);
            var dissId = await _dissLogic.CreateDiss(dto, ct);
            return Ok(new SuccessResponse<int>(dissId));
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponse(ExceptionCodes.InternalServerError, ex.Message));
        }
    }

    [HttpGet("get-diss")]
    public async Task<IActionResult> GetDiss(int id, CancellationToken ct)
    {
        try
        {
            // TODO: setup authorId (for check permision)
            var dto = await _dissLogic.GetDiss(id, ct);
            var response = _mapper.Map<GetDissResponse>(dto);
            return Ok(new SuccessResponse<GetDissResponse>(response));
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponse(ExceptionCodes.InternalServerError, ex.Message));
        }
    }

    [HttpGet("get-all-disses")]
    public async Task<IActionResult> GetAllDisses(int textId, CancellationToken ct)
    {
        try
        {
            // TODO: setup authorId (check if authorId match with textAuthorId)
            var dtos = await _dissLogic.GetAllDisses(textId, ct);
            var responses = dtos.Select(_mapper.Map<GetDissResponse>).ToList();
            return Ok(new SuccessResponse<List<GetDissResponse>>(responses));
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponse(ExceptionCodes.InternalServerError, ex.Message));
        }
    }

    [HttpGet("get-disses-page")]
    public async Task<IActionResult> GetDissesPage(int textId, int pageSize, int pageNumber, CancellationToken ct)
    {
        try
        {
            // TODO: setup authorId (check if authorId match with textAuthorId)
            var dtos = await _dissLogic.GetDissesPage(textId, pageSize, pageNumber, ct);
            var responses = dtos.Select(_mapper.Map<GetDissResponse>).ToList();
            return Ok(new SuccessResponse<List<GetDissResponse>>(responses));
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponse(ExceptionCodes.InternalServerError, ex.Message));
        }
    }

    [HttpDelete("delete-diss")]
    public async Task<IActionResult> DeleteDiss(int id, CancellationToken ct)
    {
        try
        {
            // TODO: setup authorId (for check permision)
            await _dissLogic.DeleteDiss(id, ct);
            return Ok(new SuccessResponse());
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponse(ExceptionCodes.InternalServerError, ex.Message));
        }
    }
}

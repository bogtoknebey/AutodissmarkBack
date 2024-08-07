using Autodissmark.API.ExceptionHandling;
using Autodissmark.API.Requests;
using Autodissmark.API.Responses;
using Autodissmark.API.Responses.BaseResponses;
using Autodissmark.Application.Text;
using Autodissmark.Application.Text.DTO;
using Autodissmark.Domain.ApplicationModels;
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
public class TextController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ITextLogic _textLogic;

    public TextController(
        IMapper mapper,
        ITextLogic textLogic
    )
    {
        _mapper = mapper;
        _textLogic = textLogic;
    }

    [HttpPost("create-text")]
    public async Task<IActionResult> CreateText([FromBody] CreateTextRequest request, CancellationToken ct)
    {
        try
        {
            // TODO: check author's permissions, compaire with clims (for User role)
            var dto = _mapper.Map<CreateTextInputDTO>(request);
            var textId = await _textLogic.CreateText(dto, ct);
            return Ok(new SuccessResponse<int>(textId));
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponse(ExceptionCodes.InternalServerError, ex.Message));
        }
    }

    [HttpGet("get-text")]
    public async Task<IActionResult> GetText(int id, CancellationToken ct)
    {
        try
        {
            // TODO: check author's permissions
            var text = await _textLogic.GetTextById(id, ct);
            return Ok(new SuccessResponse<TextModel>(text));
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponse(ExceptionCodes.InternalServerError, ex.Message));
        }
    }

    [HttpGet("get-texts-page")]
    public async Task<IActionResult> GetTextsPage(int authorId, int pageSize, int pageNumber, CancellationToken ct)
    {
        try
        {
            // TODO: check author's permissions
            var texts = await _textLogic.GetTextsPage(authorId, pageSize, pageNumber, ct);
            return Ok(new SuccessResponse<ICollection<TextModel>>(texts));
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponse(ExceptionCodes.InternalServerError, ex.Message));
        }
    }

    [HttpGet("get-all-texts")]
    public async Task<IActionResult> GetAllTexts(int authorId, CancellationToken ct)
    {
        try
        {
            // TODO: check author's permissions
            var texts = await _textLogic.GetAllTexts(authorId, ct);
            return Ok(new SuccessResponse<ICollection<TextModel>>(texts));
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponse(ExceptionCodes.InternalServerError, ex.Message));
        }
    }

    [HttpGet("get-texts-count")]
    public async Task<IActionResult> GetTextsCount(int authorId, CancellationToken ct)
    {
        try
        {
            // TODO: check author's permissions, compaire with clims (for User role)
            var count = await _textLogic.GetTextsCount(authorId, ct);
            return Ok(new SuccessResponse<int>(count));
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponse(ExceptionCodes.InternalServerError, ex.Message));
        }
    }

    [HttpGet("get-random-texts")]
    public async Task<IActionResult> GetRandomTexts(int authorId, int textsCount, CancellationToken ct)
    {
        try 
        {
            // TODO: check author's permissions, compaire with clims (for User role)
            var textMdoels = await _textLogic.GetRandomTexts(authorId, textsCount, ct);
            var textReponses = textMdoels.Select(_mapper.Map<GetRandomTextResponse>).ToList();

            return Ok(new SuccessResponse<ICollection<GetRandomTextResponse>>(textReponses));
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponse(ExceptionCodes.InternalServerError, ex.Message));
        }
    }

    [HttpPut("update-text")]
    public async Task<IActionResult> UpdateText([FromBody] UpdateTextRequest request, CancellationToken ct)
    {
        try
        {
            // TODO: check author's permissions, compaire with clims (for User role)
            var dto = _mapper.Map<UpdateTextInputDTO>(request);
            await _textLogic.UpdateText(dto, ct);
            return Ok(new SuccessResponse());
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponse(ExceptionCodes.InternalServerError, ex.Message));
        }
    }

    [HttpDelete("delete-text")]
    public async Task<IActionResult> DeleteText(int id, CancellationToken ct) // TODO: use header instead of body
    {
        try
        {
            // TODO: setup authorId (check if authorId match with textAuthorId)
            await _textLogic.DeleteText(id, ct);
            return Ok(new SuccessResponse());
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponse(ExceptionCodes.InternalServerError, ex.Message));
        }
    }
}

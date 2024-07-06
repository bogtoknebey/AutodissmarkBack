using Autodissmark.API.ExceptionHandling;
using Autodissmark.API.Requests;
using Autodissmark.API.Responses.BaseResponses;
using Autodissmark.Application.Author;
using Autodissmark.Application.Author.DTO;
using Autodissmark.Domain.ApplicationModels;
using Autodissmark.Domain.Enums;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Autodissmark.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthorController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IAuthorLogic _authorLogic;

    public AuthorController(
        IMapper mapper, 
        IAuthorLogic authorLogic
    )
    {
        _mapper = mapper;
        _authorLogic = authorLogic;
    }

    [HttpPost("register-author")]
    public async Task<IActionResult> CreateAuthor([FromBody] CreateAuthorRequest request, CancellationToken ct)
    {
        try
        {
            var dto = _mapper.Map<CreateAuthorInputDTO>(request);
            var authorId = await _authorLogic.CreateAuthor(dto, ct);
            return Ok(new SuccessResponse<int>(authorId));
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponse(ExceptionCodes.InternalServerError, ex.Message));
        }
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = nameof(Role.Admin) + "," + nameof(Role.User))]
    [HttpGet("get-author")]
    public async Task<IActionResult> GetAuthor(int id, CancellationToken ct)
    {
        try
        {
            var author = await _authorLogic.GetAuthorById(id, ct);
            return Ok(new SuccessResponse<AuthorModel>(author));
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponse(ExceptionCodes.InternalServerError, ex.Message));
        }
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = nameof(Role.Admin))]
    [HttpGet("get-authors-count")]
    public async Task<IActionResult> GetAuthorsCount(CancellationToken ct)
    {
        try
        {
            var count = await _authorLogic.GetAuthorsCount(ct);
            return Ok(new SuccessResponse<int>(count));
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponse(ExceptionCodes.InternalServerError, ex.Message));
        }
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = nameof(Role.Admin))]
    [HttpGet("get-all-authors-list")]
    public async Task<IActionResult> GetAllAuthors(CancellationToken ct)
    {
        try 
        {
            var authors = await _authorLogic.GetAllAuthors(ct);
            return Ok(new SuccessResponse<List<AuthorModel>>(authors.ToList()));
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponse(ExceptionCodes.InternalServerError, ex.Message));
        }
    }
}

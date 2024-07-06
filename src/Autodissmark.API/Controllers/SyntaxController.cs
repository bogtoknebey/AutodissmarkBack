using Autodissmark.API.ExceptionHandling;
using Autodissmark.API.Responses.BaseResponses;
using Autodissmark.Application.Syntax;
using Autodissmark.Domain.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Autodissmark.API.Controllers;

[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Authorize(Roles = nameof(Role.Admin) + "," + nameof(Role.User))]
[ApiController]
public class SyntaxController : ControllerBase
{
    private readonly ISyntaxLogic _syntaxLogic;

    public SyntaxController(
        ISyntaxLogic syntaxLogic
    ) 
    {
        _syntaxLogic = syntaxLogic;
    }

    [HttpGet("get-includes")]
    public async Task<IActionResult> GetIncludes(int authorId, int minimalLength, CancellationToken ct)
    {
        try
        {
            var includes = await _syntaxLogic.GetIncludes(authorId, minimalLength, ct);
            return Ok(new SuccessResponse<Dictionary<string, int>>(includes));
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponse(ExceptionCodes.InternalServerError, ex.Message));
        }
    }

    [HttpGet("get-author-random-words")]
    public async Task<IActionResult> GetAuthorRandomWords(int authorId, int minimalLength, int wordsCount, CancellationToken ct)
    {
        try
        {
            var randomWords = await _syntaxLogic.GetAuthorRandomWords(authorId, minimalLength, wordsCount, ct);
            return Ok(new SuccessResponse<ICollection<string>>(randomWords));
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponse(ExceptionCodes.InternalServerError, ex.Message));
        }
    }
}

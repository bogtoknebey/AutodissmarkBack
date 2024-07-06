using Autodissmark.API.ExceptionHandling;
using Autodissmark.API.Requests;
using Autodissmark.API.Responses.BaseResponses;
using Autodissmark.Domain.Enums;
using Autodissmark.ExternalServices.Translate.Contracts;
using Autodissmark.ExternalServices.Translate.GoogleTranslate;
using Autodissmark.TextProcessor.TextProcessor;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Autodissmark.API.Controllers;

[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Authorize(Roles = nameof(Role.Admin) + "," + nameof(Role.User))]
[ApiController]
public class TextProccessingController : ControllerBase
{
    private readonly ITranslate _translate;
    private readonly ITextProcessorLogic _textProcessorLogic;

    public TextProccessingController(
        ITranslate translate, 
        ITextProcessorLogic textProcessorLogic
    )
    {
        _translate = translate;
        _textProcessorLogic = textProcessorLogic;
    }

    [HttpPost("switch-translate-text")]
    public async Task<IActionResult> SwitchTranslateText([FromBody] SwitchTranslateTextRequest request, CancellationToken ct)
    {
        try
        {
            var translate = await _translate.GetText(request.Text, request.SwitchTimes, Language.Russian, request.SwitchLanguage);
            return Ok(new SuccessResponse<string>(translate));
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponse(ExceptionCodes.InternalServerError, ex.Message));
        }

    }

    [HttpPost("expand-text")]
    public async Task<IActionResult> ExpandText([FromBody] ExpandTextRequest request, CancellationToken ct)
    {
        try
        {
            var resultText = _textProcessorLogic.ExpandText(request.Text, ct);
            return Ok(new SuccessResponse<string>(resultText));
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponse(ExceptionCodes.InternalServerError, ex.Message));
        }
    }

    [HttpPost("add-target-to-text")]
    public async Task<IActionResult> AddTargetToText([FromBody] AddTargetToTextRequest request, CancellationToken ct)
    {
        try
        {
            var resultText = _textProcessorLogic.AddTargetToText(request.Text, request.Target, ct);
            return Ok(new SuccessResponse<string>(resultText));
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponse(ExceptionCodes.InternalServerError, ex.Message));
        }
    }

    [HttpGet("get-random-words")]
    public async Task<IActionResult> GetRandomWords(int dictionaryId, int wordsCount, CancellationToken ct)
    {
        try
        {
            var words = await _textProcessorLogic.GetRandomWords(dictionaryId, wordsCount, ct);
            return Ok(new SuccessResponse<ICollection<string>>(words));
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponse(ExceptionCodes.InternalServerError, ex.Message));
        }
    }

    [HttpGet("generate-text")]
    public async Task<IActionResult> GenerateText(int linesCount, int wordsInLineCount, CancellationToken ct)
    {
        try
        {
            string text = await _textProcessorLogic.GenerateRandomText(linesCount, wordsInLineCount, ct);
            return Ok(new SuccessResponse<string>(text));
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponse(ExceptionCodes.InternalServerError, ex.Message));
        }
    }
}

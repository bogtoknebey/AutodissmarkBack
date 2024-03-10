using Autodissmark.API.Responses.BaseResponses;
using Autodissmark.ExternalServices.Translate.Contracts;
using Autodissmark.ExternalServices.Translate.GoogleTranslate;
using Microsoft.AspNetCore.Mvc;

namespace Autodissmark.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExternalServicesController : ControllerBase
{
    private readonly ITranslate _translate;

    public ExternalServicesController(ITranslate translate)
    {
        _translate = translate;
    }

    [HttpGet("get-translate")]
    public async Task<IActionResult> GetTranslate(string text)
    {
        var translate = await _translate.GetText(text, 8, Language.Russian, Language.Chinese);
        return Ok(new SuccessResponse<string>(translate));
    }
}

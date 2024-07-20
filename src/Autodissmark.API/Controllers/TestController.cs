using Autodissmark.API.Responses.BaseResponses;
using Microsoft.AspNetCore.Mvc;

namespace Autodissmark.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    [HttpGet("test")]
    public async Task<IActionResult> Test()
    {
        return Ok(new SuccessResponse<string>("It works! Everything is alright!!!"));
    }
}

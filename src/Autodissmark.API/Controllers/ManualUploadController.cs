using Autodissmark.Domain.Options;
using Autodissmark.API.Responses.BaseResponses;
using Autodissmark.Application.ManualUpload;
using Autodissmark.TextProcessor.ManuallyUpload;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Autodissmark.API.ExceptionHandling;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Autodissmark.Domain.Enums;

namespace Autodissmark.API.Controllers;

[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Authorize(Roles = nameof(Role.Admin))]
[ApiController]
public class ManualUploadController : ControllerBase
{
    private readonly FilePathOptions _filePathOptions;
    private readonly ITextProcessorManuallyUploadLogic _textProcessorManuallyUploadLogic;
    private readonly IApplicationManualUploadLogic _applicationManuallyUploadLogic;


    public ManualUploadController(
        IOptions<FilePathOptions> filePathOptions, 
        ITextProcessorManuallyUploadLogic textProcessorManuallyUploadLogic,
        IApplicationManualUploadLogic applicationManuallyUploadLogic
    )
    {
        _filePathOptions = filePathOptions.Value;
        _textProcessorManuallyUploadLogic = textProcessorManuallyUploadLogic;
        _applicationManuallyUploadLogic = applicationManuallyUploadLogic;
    }

    [HttpPost("upload-dictionaries")]
    public async Task<IActionResult> UploadDictionaries()
    {
        try
        {
            var path = _filePathOptions.DictionariesFolderPath;
            var count = await _textProcessorManuallyUploadLogic.UploadDictionaries(path);
            return Ok(new SuccessResponse<int>(count));
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponse(ExceptionCodes.InternalServerError, ex.Message));
        }
    }

    [HttpPost("upload-beates")]
    public async Task<IActionResult> UploadBeats()
    {
        try
        {
            var path = _filePathOptions.BeatsFolderPath;
            var count = await _applicationManuallyUploadLogic.UploadBeats(path);
            return Ok(new SuccessResponse<int>(count));
        }
        catch (Exception ex)
        {
            return BadRequest(new ErrorResponse(ExceptionCodes.InternalServerError, ex.Message));
        }
    }
}

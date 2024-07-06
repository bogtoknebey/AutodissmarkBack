using Autodissmark.Domain.Enums;
using Autodissmark.Domain.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Autodissmark.API.Controllers;

[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Authorize(Roles = nameof(Role.Admin))]
[ApiController]
public class DataController : ControllerBase
{
    private readonly FilePathOptions _filePathOptions;

    public DataController(IOptions<FilePathOptions> filePathOptions)
    {
        _filePathOptions = filePathOptions.Value;
    }

    private async Task<byte[]> GetFileBytes(string path, string URI, string fileExtension)
    {
        string filePath = Path.Combine(path, $"{URI}.{fileExtension}");

        if (!System.IO.File.Exists(filePath))
        {
            return null;
        }

        return await System.IO.File.ReadAllBytesAsync(filePath);
    }

    [HttpGet("dictionaries")]
    public async Task<IActionResult> GetDictionary(string URI)
    {
        try
        {
            byte[] fileBytes = await GetFileBytes(_filePathOptions.DictionariesFolderPath, URI, "txt");
            return File(fileBytes, "text/plain", $"{URI}.txt");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("beats")]
    public async Task<IActionResult> GetBeat(string URI)
    {
        try
        {
            byte[] fileBytes = await GetFileBytes(_filePathOptions.BeatsFolderPath, URI, "weba");
            return File(fileBytes, "audio/weba", $"{URI}.weba");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}

using BookstoreApplication.DTOs.Comics;
using BookstoreApplication.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookstoreApplication.Controllers;

[Route("api/comics")]
[ApiController]
[Authorize(Roles = "Urednik")]
public class ComicsController : ControllerBase
{
    private readonly IComicsService _svc;
    private readonly ILogger<ComicsController> _log;
    private readonly IHostEnvironment _env;

    public ComicsController(IComicsService svc, ILogger<ComicsController> log, IHostEnvironment env)
    {
        _svc = svc;
        _log = log;
        _env = env;
    }

    [HttpGet("volumes")]
    [ProducesResponseType(typeof(IEnumerable<VolumeSearchItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status502BadGateway)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SearchVolumes([FromQuery] string q)
    {
        if (string.IsNullOrWhiteSpace(q))
            return BadRequest(new { detail = "Query is required." });

        try
        {
            _log.LogInformation("SearchVolumes called. q={Query}", q);
            var res = await _svc.SearchVolumesAsync(q);
            return Ok(res);
        }
        catch (HttpRequestException ex)
        {
            // greška pri pozivu ComicVine-a
            _log.LogError(ex, "ComicVine HTTP failure in SearchVolumes. q={Query}", q);
            return Problem(
                title: "ComicVine call failed",
                detail: _env.IsDevelopment() ? ex.ToString() : ex.Message,
                statusCode: StatusCodes.Status502BadGateway);
        }
        catch (Exception ex)
        {
            _log.LogError(ex, "SearchVolumes failed. q={Query}", q);
            return Problem(
                title: "Unexpected server error",
                detail: _env.IsDevelopment() ? ex.ToString() : ex.Message,
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("volumes/{volumeId:long}/issues")]
    [ProducesResponseType(typeof(IEnumerable<IssueSearchItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status502BadGateway)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetIssues(long volumeId)
    {
        try
        {
            _log.LogInformation("GetIssues called. volumeId={VolumeId}", volumeId);
            var res = await _svc.GetIssuesByVolumeAsync(volumeId);
            return Ok(res);
        }
        catch (HttpRequestException ex)
        {
            _log.LogError(ex, "ComicVine HTTP failure in GetIssues. volumeId={VolumeId}", volumeId);
            return Problem(
                title: "ComicVine call failed",
                detail: _env.IsDevelopment() ? ex.ToString() : ex.Message,
                statusCode: StatusCodes.Status502BadGateway);
        }
        catch (Exception ex)
        {
            _log.LogError(ex, "GetIssues failed. volumeId={VolumeId}", volumeId);
            return Problem(
                title: "Unexpected server error",
                detail: _env.IsDevelopment() ? ex.ToString() : ex.Message,
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}

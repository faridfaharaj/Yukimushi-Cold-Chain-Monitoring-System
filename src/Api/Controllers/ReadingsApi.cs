using System.ComponentModel.DataAnnotations;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Readings;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReadingsApi : ControllerBase
{
    private const int MIN_ID = 1;

    private readonly IReadingService _readingService;

    public ReadingsApi(IReadingService readingService)
    {
        _readingService = readingService;
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> delete(
        [Range(MIN_ID, int.MaxValue)] int id,
        CancellationToken cancellationToken
    )
    {
        bool deleted = await _readingService.deleteAsync(id, cancellationToken);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpGet("readings/{device}/{start:datetime}/{end:datetime}")]
    public async Task<ActionResult<IEnumerable<Reading>>> getRangeDevice(
        string device,
        DateTime start,
        DateTime end,
        CancellationToken cancellationToken
    )
    {
        if (start > end)
        {
            return BadRequest("start must be earlier than or equal to end.");
        }

        var readings = await _readingService.getByDateRangeAsync(
            device,
            start,
            end,
            cancellationToken
        );
        return Ok(readings);
    }
}

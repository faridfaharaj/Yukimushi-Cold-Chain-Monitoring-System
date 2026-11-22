using System.ComponentModel.DataAnnotations;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Reading>>> GetByRange(
        [FromQuery, BindRequired] DateTime start,
        [FromQuery, BindRequired] DateTime end,
        [FromQuery] string? device,
        [FromQuery] string? group,
        CancellationToken cancellationToken
    )
    {
        if (start > end)
        {
            return BadRequest("start must be earlier than or equal to end.");
        }

        var readings = await _readingService.getByDateRangeAsync(
            start,
            end,
            device,
            group,
            cancellationToken
        );
        return Ok(readings);
    }
}

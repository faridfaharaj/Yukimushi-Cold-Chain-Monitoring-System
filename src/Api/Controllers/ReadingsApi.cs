using System.ComponentModel.DataAnnotations;
using Api.Dtos;
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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReadingResponse>>> getAll(
        CancellationToken cancellationToken
    )
    {
        var readings = await _readingService.getAllAsync(cancellationToken);
        return Ok(readings.Select(toResponse));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ReadingResponse>> getById(
        [Range(MIN_ID, int.MaxValue)] int id,
        CancellationToken cancellationToken
    )
    {
        var reading = await _readingService.getByIdAsync(id, cancellationToken);
        if (reading == null)
        {
            return NotFound();
        }

        return Ok(toResponse(reading));
    }

    [HttpPost]
    public async Task<IActionResult> create(
        ReadingRequest request,
        CancellationToken cancellationToken
    )
    {
        int newId = await _readingService.createAsync(toEntity(request), cancellationToken);

        return CreatedAtAction(nameof(getById), new { id = newId }, null);
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

    private static Reading toEntity(ReadingRequest request)
    {
        return new Reading { value = request.Value };
    }

    private static ReadingResponse toResponse(Reading reading)
    {
        return new ReadingResponse { Value = reading.value, CreatedAt = reading.timeStamp };
    }
}

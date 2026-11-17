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

    private readonly IReadingService _productService;

    public ReadingsApi(IReadingService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReadingResponse>>> getAll(
        CancellationToken cancellationToken
    )
    {
        var products = await _productService.getAllAsync(cancellationToken);
        return Ok(products.Select(toResponse));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ReadingResponse>> getById(
        [Range(MIN_ID, int.MaxValue)] int id,
        CancellationToken cancellationToken
    )
    {
        var product = await _productService.getByIdAsync(id, cancellationToken);
        if (product == null)
        {
            return NotFound();
        }

        return Ok(toResponse(product));
    }

    [HttpPost]
    public async Task<IActionResult> create(
        ReadingRequest request,
        CancellationToken cancellationToken
    )
    {
        int newId = await _productService.createAsync(toEntity(request), cancellationToken);

        return CreatedAtAction(nameof(getById), new { id = newId }, null);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> delete(
        [Range(MIN_ID, int.MaxValue)] int id,
        CancellationToken cancellationToken
    )
    {
        bool deleted = await _productService.deleteAsync(id, cancellationToken);
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

    private static ReadingResponse toResponse(Reading product)
    {
        return new ReadingResponse { Value = product.value, CreatedAt = product.timeStamp };
    }
}

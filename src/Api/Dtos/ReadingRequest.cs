using System.ComponentModel.DataAnnotations;

namespace Api.Dtos;

public record ReadingRequest
{
    [Range(0, double.MaxValue, MinimumIsExclusive = true)]
    public decimal Value { get; init; }
}

namespace Api.Dtos;

public record ReadingResponse
{
    public int Id { get; init; }
    public DateTime CreatedAt { get; init; }
    public decimal Value { get; init; }
}

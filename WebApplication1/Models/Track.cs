namespace WebApplication1.Models;

public sealed class Track
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public required string Artist { get; init; }
    public required string Album { get; init; }
    public required int ReleaseYear { get; init; }
    public required ulong ListensCount { get; init; }
}

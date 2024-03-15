using Sieve.Attributes;

namespace CheckSieve.Models;

public sealed class Track
{
    [Sieve(CanFilter = true)]
    public required Guid Id { get; init; }
    
    [Sieve(CanFilter = true)]
    public required string Title { get; init; }
    
    [Sieve(CanFilter = true)]
    public required string Artist { get; init; }
    
    [Sieve(CanFilter = true)]
    public required string Album { get; init; }
    
    [Sieve(CanFilter = true)]
    public required int ReleaseYear { get; init; }
    
    [Sieve(CanFilter = true)]
    public required ulong ListensCount { get; init; }
}

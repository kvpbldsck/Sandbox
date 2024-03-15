using CheckSieve.Database;
using CheckSieve.Models;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;

namespace CheckSieve.Services;

public sealed class TrackService
{
    private readonly AppDbContext _dbContext;
    private readonly SieveProcessor _sieveProcessor;

    public TrackService(AppDbContext dbContext, SieveProcessor sieveProcessor)
    {
        _dbContext = dbContext;
        _sieveProcessor = sieveProcessor;
    }

    public async Task<List<Track>> GetTracksAsync(SieveModel sieveModel)
    {
        var queryRoot = _dbContext.Tracks.AsNoTracking();
        var query = _sieveProcessor.Apply(sieveModel, queryRoot);
        var tracks = await query.ToListAsync();
        return tracks;
    }
}

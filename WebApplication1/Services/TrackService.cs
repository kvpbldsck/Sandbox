using WebApplication1.Models;

namespace WebApplication1.Services;

public sealed class TrackService : ITrackService
{
    public Task<Track?> CreateAsync(Track track)
    {
        throw new NotImplementedException();
    }

    public Task<Track?> UpdateAsync(Track track)
    {
        throw new NotImplementedException();
    }

    public Task<Track?> GetAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<Track>> GetAsync()
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}

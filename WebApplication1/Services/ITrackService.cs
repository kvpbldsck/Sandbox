using WebApplication1.Models;

namespace WebApplication1.Services;

public interface ITrackService
{
    Task<Track?> CreateAsync(Track track);
    Task<Track?> UpdateAsync(Track track);
    Task<Track?> GetAsync(Guid id);
    Task<ICollection<Track>> GetAsync();
    Task DeleteAsync(Guid id);
}

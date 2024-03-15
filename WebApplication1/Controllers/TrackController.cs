using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers;

[ApiController]
[Route("/api/track")]
public sealed class TrackController : ControllerBase
{
    private readonly ITrackService _trackService;

    public TrackController(ITrackService trackService)
    {
        _trackService = trackService;
    }

    [HttpPost]
    public async Task<Track?> CreateTrack([FromBody] Track track)
    {
        return await _trackService.CreateAsync(track);
    }

    [HttpPut]
    public async Task<Track?> UpdateTrack([FromBody] Track track)
    {
        return await _trackService.UpdateAsync(track);
    }

    [HttpGet]
    public async Task<ICollection<Track>> GetTracks()
    {
        return await _trackService.GetAsync();
    }

    [HttpGet("{id:guid}")]
    public async Task<Track?> GetTrack([FromRoute] Guid id)
    {
        return await _trackService.GetAsync(id);
    }

    [HttpDelete("{id:guid}")]
    public async Task DeleteTrack([FromRoute] Guid id)
    {
        await _trackService.DeleteAsync(id);
    }
}

using CheckSieve.Models;
using CheckSieve.Services;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;

namespace CheckSieve.Controllers;

[ApiController]
[Route("/api/track")]
public sealed class TrackController : ControllerBase
{
    private readonly TrackService _trackService;

    public TrackController(TrackService trackService)
    {
        _trackService = trackService;
    }

    [HttpGet]
    public async Task<List<Track>> GetTracks([FromQuery]SieveModel sieveModel)
    {
        return await _trackService.GetTracksAsync(sieveModel);
    }
    
    [HttpGet("{id}/route-info/{id2}")]
    public async Task<IActionResult> GetRouteInfo([FromRoute] int id, [FromRoute] int id2)
    {
        var result = new
        {
            HttpContext.Request.Path,
            ControllerContext.ActionDescriptor.EndpointMetadata,
            ControllerContext.ActionDescriptor.ControllerTypeInfo,
            ControllerContext.ActionDescriptor.AttributeRouteInfo,
            id2,
            id
        };

        return Ok(result);
    }
}

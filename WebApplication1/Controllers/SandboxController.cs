using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Refit;
using WebApplication1.Models;
using WebApplication1.Services;
using WebApplication1.Settings;

namespace WebApplication1.Controllers;

[ApiController]
[Route("/api/sandbox")]
public sealed class SandboxController : ControllerBase
{
    record Model(int Id)
    {
        public static Model GenRandom()
        {
            return new Model(Random.Shared.Next(100));
        }
    }
    
    [HttpGet("is-array-changed")]
    public async Task<object> IsArrayChanged()
    {
        var m1 = Model.GenRandom();
        var m2 = Model.GenRandom();
        var m3 = Model.GenRandom();

        var col1 = new[] { m1, m2, m3 };
        var col2 = new[] { m1, m2, m3 };
        var col3 = new[] { m1, m3 };

        bool IsChanged1(Model[] existing, Model[] @new)
        {
            return existing
                .Select(item => @new.Any(r => r.Id == item.Id))
                .Any(sameItemExists => !sameItemExists);
        }

        bool IsChanged3(Model[] existing, Model[] @new)
        {
            return existing
                .Any(item => !@new.Any(r => r.Id == item.Id));
        }

        return new
        {
            IsChanged1_1 = IsChanged1(col1, col2),  // false  
            IsChanged3_1 = IsChanged3(col1, col2),  // false
            IsChanged1_2 = IsChanged1(col1, col3),  // true
            IsChanged3_2 = IsChanged3(col1, col3),  // true
            IsChanged1_3 = IsChanged1(col3, col1),  // false
            IsChanged3_3 = IsChanged3(col3, col1),  // false
        };
    }
    
    [HttpGet("refit-timeout")]
    public async Task<object> RefitTimeout()
    {
        var cancellationTokenRefit = RestService.For<IGoogleRefit>("https://googl.com", new RefitSettings());
        var cancelSource = new CancellationTokenSource(TimeSpan.FromTicks(1));
        
        var httpClientRefit = RestService.For<IGoogleRefit>(new HttpClient 
        {
            BaseAddress = new Uri("https://googl.com"),
            Timeout = TimeSpan.FromTicks(1)
        });

        return new
        {
            // CancellationTokenTimeout = await cancellationTokenRefit.GetMainPage(cancelSource.Token),
            HttpClientTimeout = await httpClientRefit.GetMainPage(default),
        };
    }
    
    [HttpGet("set-check")]
    public object SetCheck([FromServices] IOptions<SetCheckSettings> settings)
    {
        return settings.Value;
    }
    
    [HttpGet("dict-check")]
    public object DictCheck([FromServices] IOptions<DictCheckSettings> settings)
    {
        return settings.Value;
    }
    
    [HttpGet("catch-mult")]
    public object CatchMult()
    {
        try
        {
            throw Random.Shared.Next() % 2 == 0
                ? new JsonException("System.json exception")
                : new Newtonsoft.Json.JsonException("Newtonsoft");
        }
        catch (Exception e) when (e is Newtonsoft.Json.JsonException or JsonException)
        {
            return "Caught " + e.Message;
        }
    }
}

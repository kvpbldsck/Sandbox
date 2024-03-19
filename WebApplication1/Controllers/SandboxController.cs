using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Refit;
using WebApplication1.Extensions;
using WebApplication1.Models;
using WebApplication1.Services;
using WebApplication1.Settings;
using Formatting = Newtonsoft.Json.Formatting;
using JsonException = System.Text.Json.JsonException;

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
    
    [HttpPost("check-json")]
    public object CheckJson([FromBody] Dictionary<string, object> whatToWrite)
    {
        try
        {
            var json = new JObject();
            foreach (var (key, value) in whatToWrite)
            {
                json.WriteAtPath(key, JToken.FromObject(value));
            }
            return json.ToString(Formatting.Indented);
        }
        catch (Exception e)
        {
            return "Caught " + e.Message;
        }
    }
    
    [HttpGet("check-xml")]
    public object CheckXml()
    {
        var withTrue = new XmlTest { IsCancelledForever = true };
        var withFalse = new XmlTest { IsCancelledForever = false };
        var withNull = new XmlTest { IsCancelledForever = null };
        
        using var writer1 = new MemoryStream();
        using var writer2 = new MemoryStream();
        using var writer3 = new MemoryStream();
        using var writer4 = new MemoryStream(Encoding.UTF8.GetBytes(@"<SandboxController.XmlTest xmlns=""http://schemas.datacontract.org/2004/07/WebApplication1.Controllers"" xmlns:i=""http://www.w3.org/2001/XMLSchema-instance""></SandboxController.XmlTest>"));
        
        var serializer = new DataContractSerializer(typeof(XmlTest), new DataContractSerializerSettings() {  });

        serializer.WriteObject(writer1, withTrue);
        serializer.WriteObject(writer2, withFalse);
        serializer.WriteObject(writer3, withNull);

        writer1.Seek(0, SeekOrigin.Begin);
        writer2.Seek(0, SeekOrigin.Begin);
        writer3.Seek(0, SeekOrigin.Begin);
        writer4.Seek(0, SeekOrigin.Begin);
        
        var serializedWithTrue = Encoding.UTF8.GetString(writer1.ToArray());
        var serializedWithFalse = Encoding.UTF8.GetString(writer2.ToArray());
        var serializedWithNull = Encoding.UTF8.GetString(writer3.ToArray());
        var serializedWithout = Encoding.UTF8.GetString(writer4.ToArray());
        
        writer1.Seek(0, SeekOrigin.Begin);
        writer2.Seek(0, SeekOrigin.Begin);
        writer3.Seek(0, SeekOrigin.Begin);
        writer4.Seek(0, SeekOrigin.Begin);
        
        using var reader1 = new StringReader(serializedWithTrue);
        using var reader2 = new StringReader(serializedWithFalse);
        using var reader3 = new StringReader(serializedWithNull);
        using var reader4 = new StringReader(serializedWithout);
        
        var deserializedWithTrue = serializer.ReadObject(writer1);
        var deserializedWithFalse = serializer.ReadObject(writer2);
        var deserializedWithNull = serializer.ReadObject(writer3);
        var deserializedWithout = serializer.ReadObject(writer4);

        return new
        {
            withTrue,
            withFalse,
            withNull,

            serializedWithTrue,
            serializedWithFalse,
            serializedWithNull,
            serializedWithout,

            deserializedWithTrue,
            deserializedWithFalse,
            deserializedWithNull,
            deserializedWithout
        };
    }

    public class XmlTest
    {
        [DataMember(EmitDefaultValue = false)]
        public bool? IsCancelledForever { get; set; }
    }
}

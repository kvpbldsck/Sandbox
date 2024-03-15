using System.Text.RegularExpressions;
using CheckSieve.Database;
using CheckSieve.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Sieve.Models;
using Sieve.Services;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<SieveOptions>(builder.Configuration.GetSection("Sieve"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.OperationFilter<SnakecasingParameOperationFilter>();
});
builder.Services.AddScoped<TrackService>();
builder.Services.AddScoped<SieveProcessor>();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite($"Data Source=./db.sqlite");
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.UseHttpsRedirection();

app.Run();


public class SnakecasingParameOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.Parameters == null) operation.Parameters = new List<OpenApiParameter>();
        else { 
            foreach(var item in operation.Parameters)
            {             
                // item.Name = ToSnakeCase(item.Name);
            }              
        }
    }

    private static string ToSnakeCase(string o) =>
        Regex.Replace(o, @"(\w)([A-Z])", "$1_$2").ToLower();
}

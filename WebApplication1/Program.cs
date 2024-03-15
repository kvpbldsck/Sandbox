using System.Text.RegularExpressions;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebApplication1.Services;
using WebApplication1.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<SetCheckSettings>(builder.Configuration.GetSection(SetCheckSettings.Section));
builder.Services.Configure<DictCheckSettings>(builder.Configuration.GetSection(DictCheckSettings.Section));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.OperationFilter<SnakecasingParameOperationFilter>();
});
builder.Services.AddScoped<ITrackService, TrackService>();
// builder.Services.AddDbContext<AppDbContext>(options =>
// {
//     options.UseSqlite(builder.Configuration.GetConnectionString("Sqlite"));
// });

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
// {
//     var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
//     context.Database.Migrate();
// }

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

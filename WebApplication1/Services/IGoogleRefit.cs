using Refit;

namespace WebApplication1.Services;

public interface IGoogleRefit
{
    [Get("/")]
    Task<ApiResponse<string>> GetMainPage(CancellationToken cancellationToken);
}

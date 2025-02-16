using Asp.Versioning;

namespace bingo_api.src.Extensions;

public static class ApiVersioningSetup
{
    public static void AddVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
            options.ApiVersionReader = ApiVersionReader.Combine(
             new QueryStringApiVersionReader("api-version"),
             new HeaderApiVersionReader("X-Version"),
             new MediaTypeApiVersionReader("ver")
            );
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });
    }
}

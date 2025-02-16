
using Hellang.Middleware.ProblemDetails;
using Hellang.Middleware.ProblemDetails.Mvc;

namespace bingo_api.src.Extensions;

public static class ProblemDetailsSetup
{
    public static void AddApiProblemDetails(this IServiceCollection services)
    {
        services.AddProblemDetails(options =>
        {
            options.IncludeExceptionDetails = (ctx, ex) =>
            {
                var env = ctx.RequestServices.GetRequiredService<IHostEnvironment>();
                return env.IsDevelopment() || env.IsStaging();
            };

            options.MapToStatusCode<NotImplementedException>(StatusCodes.Status501NotImplemented);
            options.MapToStatusCode<HttpRequestException>(StatusCodes.Status503ServiceUnavailable);
            options.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);
        })
        .AddProblemDetailsConventions();
    }

}
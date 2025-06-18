
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;

namespace bingo_api.src.Extensions;

public static class ProblemDetailsSetup
{
    private static Dictionary<Type, int> _mapping = new Dictionary<Type, int>
    {
        { typeof(UnauthorizedAccessException), StatusCodes.Status401Unauthorized },
        { typeof(JsonException), StatusCodes.Status400BadRequest },
        { typeof(ArgumentException), StatusCodes.Status400BadRequest },
        { typeof(ArgumentNullException), StatusCodes.Status400BadRequest },
        { typeof(NotImplementedException), StatusCodes.Status501NotImplemented },
        { typeof(HttpRequestException), StatusCodes.Status503ServiceUnavailable },
        { typeof(Exception), StatusCodes.Status500InternalServerError },
    };
    public static void AddApiProblemDetails(this IServiceCollection services)
    {
        services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = (context) => context.MapExceptionToStatusCode();
        });
    }

    public static void MapExceptionToStatusCode(this ProblemDetailsContext context)
    {
        var env = context.HttpContext.RequestServices.GetRequiredService<IHostEnvironment>();
        var exception = context.HttpContext.Features.Get<IExceptionHandlerPathFeature>()?.Error;
        var isProduction = context.HttpContext.RequestServices.GetRequiredService<IWebHostEnvironment>().IsProduction();
          // Adiciona o TraceId para correlação de logs
        var traceId = System.Diagnostics.Activity.Current?.Id ?? context.HttpContext.TraceIdentifier;
        context.ProblemDetails.Extensions["traceId"] = traceId;

        // Em produção, não expõe detalhes da exceção
        if (isProduction && exception is not null)
        {
            context.ProblemDetails.Title = "Ocorreu um erro inesperado.";
            context.ProblemDetails.Detail = exception.Message;
        }
        else if (exception is not null) // Para ambiente de Desenvolvimento
        {
            var statusCode = _mapping.GetValueOrDefault(exception.GetType(), context.HttpContext.Response.StatusCode);
            context.HttpContext.Response.StatusCode = statusCode;
            context.ProblemDetails.Status = statusCode;
            context.ProblemDetails.Detail = context.ProblemDetails.Detail == "" ? context.ProblemDetails.Detail: exception.Message;
            context.ProblemDetails.Extensions["stackTrace"] = exception.StackTrace;
        }

/*
        if (exception is not null)
        {
            var statusCode = _mapping.GetValueOrDefault(exception.GetType(), context.HttpContext.Response.StatusCode);
            context.HttpContext.Response.StatusCode = statusCode;
            context.ProblemDetails.Status = statusCode;
            context.ProblemDetails.Detail = env.IsProduction() || env.IsStaging() ? context.ProblemDetails.Detail : null;
        }
        */
    }
}
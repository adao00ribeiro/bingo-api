
using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using bingo_api.src.SwaggerFilter;
using bingo_api.src.Enums;
using Microsoft.OpenApi.Any;
using Asp.Versioning.ApiExplorer;


namespace bingo_api.src.Extensions;

public static class SwaggerSetup
{
    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);
            options.DocumentFilter<TagDescriptionsDocumentFilter>();
            options.MapType<EPrizeType>(() => new OpenApiSchema
            {
                Type = "string",
                Enum = Enum.GetNames(typeof(EPrizeType))
                    .Select(name => new OpenApiString(name))
                    .Cast<IOpenApiAny>()
                    .ToList()
            });

            options.UseInlineDefinitionsForEnums();

            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "bingo_api",
                Version = "v1",

            });
            options.SwaggerDoc("v2", new OpenApiInfo
            {
                Title = "bingo_api",
                Version = "v2"
            });
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = @"JWT Authorization header using the Bearer scheme. 
                                Enter 'Bearer' [space] and then your token in the text input below. 
                                Example: 'Bearer 12345abcdef'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header,

                    },
                    new List<string>()
                }
            });

        });
    }
    public static void UseSwaggerUI(this WebApplication app)
    {
        app.UseSwagger();

        app.UseSwaggerUI(options =>
        {
            var apiVersionProvider = app.Services.GetService<IApiVersionDescriptionProvider>();
            if (apiVersionProvider == null)
                throw new ArgumentException("API Versioning not registered.");

            foreach (var description in apiVersionProvider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json",
                description.GroupName);
            }
            options.RoutePrefix = string.Empty;
            options.EnableFilter();
            options.DocExpansion(DocExpansion.List);
        });
    }

}

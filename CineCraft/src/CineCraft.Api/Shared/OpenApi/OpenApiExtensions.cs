using Microsoft.OpenApi;
using Microsoft.AspNetCore.OpenApi;

namespace CineCraft.Api.Shared.OpenApi;

public static class OpenApiExtensions
{
    public static IHostApplicationBuilder AddCineCraftOpenApi(this IHostApplicationBuilder builder)
    {
        builder.Services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer<BearerSecurityDocumentTransformer>();
        });

        return builder;
    }

    public static WebApplication UseCineCraftSwaggerUI(this WebApplication app)
    {
        app.MapOpenApi();

        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/openapi/v1.json", "CineCraft API v1");
            options.EnablePersistAuthorization();
        });

        return app;
    }
}

/// <summary>
/// Document transformer that adds Bearer authentication scheme to OpenAPI spec.
/// </summary>
internal sealed class BearerSecurityDocumentTransformer : IOpenApiDocumentTransformer
{
    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        document.Info ??= new OpenApiInfo();
        document.Info.Title = "CineCraft API";

        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();

        document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Description = "Ingresa tu JWT token obtenido de POST /api/login"
        };

        if (document.Paths is not null)
        {
            foreach (var path in document.Paths.Values)
            {
                if (path.Operations is null) continue;
                foreach (var kvp in path.Operations)
                {
                    var operation = kvp.Value;
                    operation.Security ??= [];
                    operation.Security.Add(new OpenApiSecurityRequirement
                    {
                        [new OpenApiSecuritySchemeReference("Bearer", document)] = new List<string>()
                    });
                }
            }
        }

        return Task.CompletedTask;
    }
}
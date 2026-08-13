using Azure.Identity;
using CineCraft.Api.Data;
using CineCraft.Api.Features.Auth;
using CineCraft.Api.Features.Solicitudes;
using CineCraft.Api.Features.Resenias;
using CineCraft.Api.Shared.Authentication;
using CineCraft.Api.Shared.Cors;
using CineCraft.Api.Shared.Database;
using CineCraft.Api.Shared.ErrorHandling;
using CineCraft.Api.Shared.OpenApi;
using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    DotNetEnv.Env.TraversePath().Load();
    builder.Configuration.AddEnvironmentVariables();
}

builder.AddServiceDefaults();

builder.Services.AddProblemDetails()
                .AddExceptionHandler<GlobalExceptionHandler>();

// Configure database options
builder.Services.AddOptions<DatabaseOptions>()
                .Bind(builder.Configuration.GetSection(DatabaseOptions.SectionName));

var credential = new DefaultAzureCredential(new DefaultAzureCredentialOptions
{
    ManagedIdentityClientId = builder.Configuration["AZURE_CLIENT_ID"]
});

builder.AddCineCraftNpgsql<CineCraftContext>("CineCraftDB", credential);

// Configure authentication options with validation
builder.Services.AddOptions<AuthOptions>()
                .Bind(builder.Configuration.GetSection(AuthOptions.SectionName))
                .ValidateDataAnnotations()
                .ValidateOnStart();

// Configure default users options
builder.Services.AddOptions<UsersOptions>()
                .Bind(builder.Configuration.GetSection(UsersOptions.SectionName));

// Register the JWT Bearer options configurator first
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();

// Register JWT token service
builder.Services.AddSingleton<JwtTokenService>();

// Then add the authentication services
builder.Services.AddAuthentication()
                .AddJwtBearer();

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("AdminOnly", policy => policy.RequireRole("admin"));

builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = HttpLoggingFields.RequestMethod |
                            HttpLoggingFields.RequestPath |
                            HttpLoggingFields.ResponseStatusCode |
                            HttpLoggingFields.Duration;
    options.CombineLogs = true;
});

builder.AddCineCraftOpenApi();

builder.AddCineCraftCors();

builder.Services.AddValidation();

var app = builder.Build();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultEndpoints();
app.MapAuth();
app.MapSolicitudes();
app.MapResenias();

app.UseHttpLogging();

if (app.Environment.IsDevelopment())
{
    app.UseCineCraftSwaggerUI();
}
else
{
    app.UseExceptionHandler();
}

app.UseStatusCodePages();

await app.MigrateDbAsync();

app.Run();
namespace CineCraft.Api.Shared.Database;

/// <summary>
/// Configuration options for Database settings.
/// </summary>
public class DatabaseOptions
{
    public const string SectionName = "Database";

    public string DB_USER { get; set; } = string.Empty;

    public string DB_PASSWORD { get; set; } = string.Empty;

    public string DB_HOST { get; set; } = string.Empty;

    public string DB_PORT { get; set; } = string.Empty;

    public string DB_NAME { get; set; } = string.Empty;

    public string BuildConnectionString()
    {
        var port = string.IsNullOrWhiteSpace(DB_PORT) ? "5432" : DB_PORT;
        var dbName = string.IsNullOrWhiteSpace(DB_NAME) ? "cinecraft" : DB_NAME;

        return $"Host={DB_HOST};Port={port};Database={dbName};Username={DB_USER};Password={DB_PASSWORD}";
    }
}

using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddAzurePostgresFlexibleServer("postgres")
                    .RunAsContainer(postgres =>
                    {
                        postgres.WithDataVolume("postgres-cinecraft-data");
                        postgres.WithPgAdmin(pgAdmin =>
                        {
                            pgAdmin.WithHostPort(5050);
                        });
                    });

var cineCraftDb = postgres.AddDatabase("CineCraftDB", "CineCraft");

builder.AddProject<CineCraft_Api>("cinecraft-api")
        .WithReference(cineCraftDb)
        .WaitFor(cineCraftDb)
        .WithHttpEndpoint(port: 5000, name: "http-api")
        .WithUrls(context =>
        {
            context.Urls.Add(new()
            {
                Url = "/swagger",
                DisplayText = "API Docs",
                Endpoint = context.GetEndpoint("http")
            });
        })
        .WithExternalHttpEndpoints()
        .WithHttpHealthCheck("/health/ready");

if (builder.ExecutionContext.IsPublishMode)
{
    var postgresUser = builder.AddParameter("PostgresUser", value: "postgres");
    var postgresPassword = builder.AddParameter("PostgresPassword", secret: true);
    postgres.WithPasswordAuthentication(userName: postgresUser, password: postgresPassword);
}

builder.AddAzureContainerAppEnvironment("cae");

builder.Build().Run();
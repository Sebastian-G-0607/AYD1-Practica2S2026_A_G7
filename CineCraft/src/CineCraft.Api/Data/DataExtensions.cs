using Azure.Core;
using CineCraft.Api.Models;
using CineCraft.Api.Shared.Authentication;
using CineCraft.Api.Shared.Database;
using Microsoft.EntityFrameworkCore;

namespace CineCraft.Api.Data;

public static class DataExtensions
{
    public static WebApplicationBuilder AddCineCraftNpgsql<TContext>(
        this WebApplicationBuilder builder,
        string connectionStringName,
        TokenCredential credential
    ) where TContext : DbContext
    {
        var connectionString = builder.Configuration.GetConnectionString(connectionStringName);

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            var dbOptions = builder.Configuration
                .GetSection(DatabaseOptions.SectionName)
                .Get<DatabaseOptions>();

            if (dbOptions != null && !string.IsNullOrWhiteSpace(dbOptions.DB_HOST))
            {
                builder.Configuration[$"ConnectionStrings:{connectionStringName}"] = dbOptions.BuildConnectionString();
            }
        }

        var usersOptions = builder.Configuration
            .GetSection(UsersOptions.SectionName)
            .Get<UsersOptions>() ?? new UsersOptions();

        if (builder.Environment.IsProduction())
        {
            builder.AddAzureNpgsqlDbContext<TContext>(
                connectionStringName,
                settings => settings.Credential = credential,
                configureDbContextOptions: options =>
                    ConfigureDbContext(options, usersOptions)
            );
        }
        else
        {
            builder.AddNpgsqlDbContext<TContext>(
                connectionStringName,
                configureDbContextOptions: options =>
                    ConfigureDbContext(options, usersOptions));
        }

        return builder;
    }

    public static async Task MigrateDbAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        CineCraftContext dbContext = scope.ServiceProvider
                                          .GetRequiredService<CineCraftContext>();
        await dbContext.Database.MigrateAsync();
    }

    private static DbContextOptionsBuilder ConfigureDbContext(DbContextOptionsBuilder options, UsersOptions usersOptions)
    {
        return options.UseSeeding((context, _) =>
                    {
                        if (!context.Set<SolicitudStatus>().Any())
                        {
                            SeedSolicitudStatuses(context);
                            context.SaveChanges();
                        }

                        if (!context.Set<Rol>().Any())
                        {
                            SeedRoles(context);
                            context.SaveChanges();
                        }

                        if (!context.Set<Usuario>().Any())
                        {
                            SeedUsuarioAdmin(context, usersOptions);
                            context.SaveChanges();
                        }
                    })
                    .UseAsyncSeeding(async (context, _, cancellationToken) =>
                    {
                        if (!context.Set<SolicitudStatus>().Any())
                        {
                            SeedSolicitudStatuses(context);
                            await context.SaveChangesAsync(cancellationToken);
                        }

                        if (!context.Set<Rol>().Any())
                        {
                            SeedRoles(context);
                            await context.SaveChangesAsync(cancellationToken);
                        }

                        if (!context.Set<Usuario>().Any())
                        {
                            SeedUsuarioAdmin(context, usersOptions);
                            await context.SaveChangesAsync(cancellationToken);
                        }
                    });
    }

    private static void SeedSolicitudStatuses(DbContext context)
    {
        context.Set<SolicitudStatus>().AddRange(
            new SolicitudStatus { Descripcion = "Pendiente" },
            new SolicitudStatus { Descripcion = "Aprobado" },
            new SolicitudStatus { Descripcion = "Rechazado" }
        );
    }

    private static void SeedRoles(DbContext context)
    {
        context.Set<Rol>().AddRange(
            new Rol { Nombre = "admin", Descripcion = "Administrador del sistema" },
            new Rol { Nombre = "estandar", Descripcion = "Usuario estándar" }
        );
    }

    private static void SeedUsuarioAdmin(DbContext context, UsersOptions usersOptions)
    {
        var adminRol = context.Set<Rol>().First(r => r.Nombre == "admin");

        var email = usersOptions.DefaultAdminEmail;

        var password = usersOptions.DefaultAdminPassword;

        context.Set<Usuario>().Add(new Usuario
        {
            Nombre = "Administrador",
            RolId = adminRol.Id,
            Contrasenia = BCrypt.Net.BCrypt.HashPassword(password),
            Correo = email,
            SolicitudId = null
        });
    }
}

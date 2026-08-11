using CineCraft.Api.Data.Configurations;
using CineCraft.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CineCraft.Api.Data;

public class CineCraftContext(DbContextOptions<CineCraftContext> options)
    : DbContext(options)
{
    public DbSet<SolicitudStatus> SolicitudStatuses => Set<SolicitudStatus>();

    public DbSet<Rol> Roles => Set<Rol>();

    public DbSet<Solicitud> Solicitudes => Set<Solicitud>();

    public DbSet<Usuario> Usuarios => Set<Usuario>();

    public DbSet<Etiqueta> Etiquetas => Set<Etiqueta>();

    public DbSet<Resenia> Resenias => Set<Resenia>();

    public DbSet<CompartirReseniaUsuario> CompartirReseniaUsuarios => Set<CompartirReseniaUsuario>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RolEntityConfiguration).Assembly);
    }
}

using CineCraft.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CineCraft.Api.Data.Configurations;

public class UsuarioEntityConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("usuario");

        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).HasColumnName("id");

        builder.Property(u => u.Nombre)
               .HasColumnName("nombre")
               .HasMaxLength(150)
               .IsRequired();

        builder.Property(u => u.RolId)
               .HasColumnName("rol_id");

        builder.Property(u => u.Contrasenia)
               .HasColumnName("contrasenia")
               .HasMaxLength(255)
               .IsRequired();

        builder.Property(u => u.Correo)
               .HasColumnName("correo")
               .HasMaxLength(150)
               .IsRequired();

        builder.HasIndex(u => u.Correo)
               .IsUnique();

        builder.Property(u => u.SolicitudId)
               .HasColumnName("solicitud_id");

        // UNIQUE constraint on solicitud_id
        builder.HasIndex(u => u.SolicitudId)
               .IsUnique()
               .HasFilter("solicitud_id IS NOT NULL");

        builder.HasOne(u => u.Rol)
               .WithMany(r => r.Usuarios)
               .HasForeignKey(u => u.RolId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(u => u.Solicitud)
               .WithOne(s => s.Usuario)
               .HasForeignKey<Usuario>(u => u.SolicitudId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}

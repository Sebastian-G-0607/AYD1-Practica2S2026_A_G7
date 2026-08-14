using CineCraft.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CineCraft.Api.Data.Configurations;

public class SolicitudEntityConfiguration : IEntityTypeConfiguration<Solicitud>
{
    public void Configure(EntityTypeBuilder<Solicitud> builder)
    {
        builder.ToTable("solicitud");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnName("id");

        builder.Property(s => s.Nombre)
               .HasColumnName("nombre")
               .HasMaxLength(150)
               .IsRequired();

        builder.Property(s => s.Correo)
               .HasColumnName("correo")
               .HasMaxLength(150)
               .IsRequired();

        builder.Property(s => s.Contrasenia)
               .HasColumnName("contrasenia")
               .HasMaxLength(255)
               .IsRequired();

        builder.Property(s => s.StatusId)
               .HasColumnName("status_id");

        builder.Property(s => s.FechaSolicitud)
               .HasColumnName("fecha_solicitud");

        builder.Property(s => s.MotivoRechazo)
               .HasColumnName("motivo_rechazo")
               .HasMaxLength(255);

        builder.HasOne(s => s.Status)
               .WithMany(st => st.Solicitudes)
               .HasForeignKey(s => s.StatusId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}

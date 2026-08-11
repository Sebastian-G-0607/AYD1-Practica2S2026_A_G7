using CineCraft.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CineCraft.Api.Data.Configurations;

public class SolicitudStatusEntityConfiguration : IEntityTypeConfiguration<SolicitudStatus>
{
    public void Configure(EntityTypeBuilder<SolicitudStatus> builder)
    {
        builder.ToTable("solicitud_status");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnName("id");

        builder.Property(s => s.Descripcion)
               .HasColumnName("descripcion")
               .HasMaxLength(255)
               .IsRequired();
    }
}

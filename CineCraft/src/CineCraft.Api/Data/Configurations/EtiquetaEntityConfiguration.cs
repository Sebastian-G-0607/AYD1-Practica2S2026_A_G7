using CineCraft.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CineCraft.Api.Data.Configurations;

public class EtiquetaEntityConfiguration : IEntityTypeConfiguration<Etiqueta>
{
    public void Configure(EntityTypeBuilder<Etiqueta> builder)
    {
        builder.ToTable("etiqueta");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id");

        builder.Property(e => e.UsuarioId)
               .HasColumnName("usuario_id");

        builder.Property(e => e.Descripcion)
               .HasColumnName("descripcion")
               .HasMaxLength(255)
               .IsRequired();

        builder.HasOne(e => e.Usuario)
               .WithMany(u => u.Etiquetas)
               .HasForeignKey(e => e.UsuarioId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}

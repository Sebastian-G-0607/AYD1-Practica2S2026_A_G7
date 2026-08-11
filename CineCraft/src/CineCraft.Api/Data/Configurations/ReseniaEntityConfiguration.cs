using CineCraft.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CineCraft.Api.Data.Configurations;

public class ReseniaEntityConfiguration : IEntityTypeConfiguration<Resenia>
{
    public void Configure(EntityTypeBuilder<Resenia> builder)
    {
        builder.ToTable("resenia");

        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).HasColumnName("id");

        builder.Property(r => r.UsuarioAutorId)
               .HasColumnName("usuario_autor_id");

        builder.Property(r => r.TituloPelicula)
               .HasColumnName("titulo_pelicula")
               .HasMaxLength(255)
               .IsRequired();

        builder.Property(r => r.Calificacion)
               .HasColumnName("calificacion");

        // CHECK constraint: calificacion >= 0 AND calificacion <= 10
        builder.ToTable(t => t.HasCheckConstraint(
            "CK_resenia_calificacion",
            "calificacion >= 0 AND calificacion <= 10"));

        builder.Property(r => r.Comentario)
               .HasColumnName("comentario")
               .HasColumnType("text");

        builder.Property(r => r.EtiquetaId)
               .HasColumnName("etiqueta_id");

        builder.Property(r => r.Destacada)
               .HasColumnName("destacada")
               .HasDefaultValue(false);

        builder.Property(r => r.Archivada)
               .HasColumnName("archivada")
               .HasDefaultValue(false);

        builder.Property(r => r.Deleted)
               .HasColumnName("deleted")
               .HasDefaultValue(false);

        builder.HasOne(r => r.UsuarioAutor)
               .WithMany(u => u.Resenias)
               .HasForeignKey(r => r.UsuarioAutorId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Etiqueta)
               .WithMany(e => e.Resenias)
               .HasForeignKey(r => r.EtiquetaId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}

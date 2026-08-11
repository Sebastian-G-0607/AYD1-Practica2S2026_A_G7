using CineCraft.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CineCraft.Api.Data.Configurations;

public class CompartirReseniaUsuarioEntityConfiguration : IEntityTypeConfiguration<CompartirReseniaUsuario>
{
    public void Configure(EntityTypeBuilder<CompartirReseniaUsuario> builder)
    {
        builder.ToTable("compartir_resenia_usuario");

        // Composite primary key
        builder.HasKey(c => new { c.ReseniaId, c.UsuarioRemitenteId, c.UsuarioDestinatarioId });

        builder.Property(c => c.ReseniaId).HasColumnName("resenia_id");
        builder.Property(c => c.UsuarioRemitenteId).HasColumnName("usuario_remitente_id");
        builder.Property(c => c.UsuarioDestinatarioId).HasColumnName("usuario_destinatario_id");

        builder.HasOne(c => c.Resenia)
               .WithMany(r => r.Compartidos)
               .HasForeignKey(c => c.ReseniaId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.UsuarioRemitente)
               .WithMany(u => u.ReseniasEnviadas)
               .HasForeignKey(c => c.UsuarioRemitenteId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.UsuarioDestinatario)
               .WithMany(u => u.ReseniasRecibidas)
               .HasForeignKey(c => c.UsuarioDestinatarioId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}

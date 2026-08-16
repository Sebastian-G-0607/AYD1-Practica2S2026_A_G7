
namespace CineCraft.Api.Features.Resenias.GetReseniasArchivadas;

public record ReseniaArchivadaDto(
    int Id,
    string TituloPelicula,
    int Calificacion,
    string? Comentario,
    int EtiquetaId,
    string Etiqueta,
    bool Destacada,
    bool Archivada
);
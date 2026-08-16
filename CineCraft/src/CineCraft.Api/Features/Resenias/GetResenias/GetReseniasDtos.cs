namespace CineCraft.Api.Features.Resenias.GetResenias;

public record ReseniaDto(
    int Id,
    string TituloPelicula,
    int Calificacion,
    string? Comentario,
    int EtiquetaId,
    string Etiqueta,
    bool Destacada,
    bool Archivada
);
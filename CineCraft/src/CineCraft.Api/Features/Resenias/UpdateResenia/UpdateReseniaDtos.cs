namespace CineCraft.Api.Features.Resenias.UpdateResenia;

public record UpdateReseniaRequest(
    string TituloPelicula,
    int Calificacion,
    string? Comentario,
    int? EtiquetaId,
    string? NuevaEtiqueta
);

public record UpdateReseniaResponse(
    int Id,
    string TituloPelicula,
    int Calificacion,
    string? Comentario,
    int EtiquetaId,
    string Etiqueta
);
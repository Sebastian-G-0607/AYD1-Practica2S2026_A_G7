namespace CineCraft.Api.Features.Resenias.CreateResenia;

public record CreateReseniaRequest(
    string TituloPelicula,
    int Calificacion,
    string? Comentario,
    int? EtiquetaId,
    string? NuevaEtiqueta
);

public record CreateReseniaResponse(
    int Id,
    string TituloPelicula,
    int Calificacion,
    string? Comentario,
    int EtiquetaId,
    string Etiqueta
);
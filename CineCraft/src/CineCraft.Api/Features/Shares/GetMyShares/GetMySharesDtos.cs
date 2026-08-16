namespace CineCraft.Api.Features.Shares.GetMyShares;

public record MySharedDto(
    int ReseniaId, 
    string TituloPelicula, 
    int Calificacion,
    string? Comentario, 
    string? Etiqueta,
    int UsuarioDestinatarioId, 
    string DestinatarioNombre,
    string DestinatarioCorreo
);
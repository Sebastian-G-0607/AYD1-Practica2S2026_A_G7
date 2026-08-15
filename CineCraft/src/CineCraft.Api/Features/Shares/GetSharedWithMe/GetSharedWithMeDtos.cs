namespace CineCraft.Api.Features.Shares.GetSharedWithMe;

public record SharedWithMeDto(
    int ReseniaId, 
    string TituloPelicula, 
    int Calificacion,
    string? Comentario, 
    string? Etiqueta,
    int UsuarioRemitenteId, 
    string RemitenteNombre,
    string RemitenteCorreo,
    int? UsuarioAutorId = null,
    string? AutorNombre = null
);
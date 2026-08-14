namespace CineCraft.Api.Features.Shares.GetSharedWithMe;

public record SharedWithMeDto(
    int ReseniaId, 
    string TituloPelicula, 
    string Comentario, 
    int UsuarioRemitenteId, 
    string RemitenteNombre
);
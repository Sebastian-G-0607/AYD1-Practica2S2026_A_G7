namespace CineCraft.Api.Features.Shares.GetMyShares;

public record MySharedDto(
    int ReseniaId, 
    string TituloPelicula, 
    string Comentario, 
    int UsuarioDestinatarioId, 
    string DestinatarioNombre
);
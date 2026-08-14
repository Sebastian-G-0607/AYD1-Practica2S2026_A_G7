namespace CineCraft.Api.Models;

public class CompartirReseniaUsuario
{
    public int ReseniaId { get; set; }

    public int UsuarioRemitenteId { get; set; }

    public int UsuarioDestinatarioId { get; set; }

    // Navigation
    public Resenia? Resenia { get; set; }

    public Usuario? UsuarioRemitente { get; set; }

    public Usuario? UsuarioDestinatario { get; set; }
}

namespace CineCraft.Api.Models;

public class Usuario
{
    public int Id { get; set; }

    public required string Nombre { get; set; }

    public int RolId { get; set; }

    public required string Contrasenia { get; set; }

    public required string Correo { get; set; }

    public int? SolicitudId { get; set; }

    // Navigation
    public Rol? Rol { get; set; }

    public Solicitud? Solicitud { get; set; }

    public ICollection<Etiqueta> Etiquetas { get; set; } = [];

    public ICollection<Resenia> Resenias { get; set; } = [];

    public ICollection<CompartirReseniaUsuario> ReseniasEnviadas { get; set; } = [];

    public ICollection<CompartirReseniaUsuario> ReseniasRecibidas { get; set; } = [];
}

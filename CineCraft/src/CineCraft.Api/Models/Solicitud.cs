namespace CineCraft.Api.Models;

public class Solicitud
{
    public int Id { get; set; }

    public required string Nombre { get; set; }

    public required string Correo { get; set; }

    public required string Contrasenia { get; set; }

    public int StatusId { get; set; }

    public DateTime FechaSolicitud { get; set; }

    public string? MotivoRechazo { get; set; }

    // Navigation
    public SolicitudStatus? Status { get; set; }

    public Usuario? Usuario { get; set; }
}

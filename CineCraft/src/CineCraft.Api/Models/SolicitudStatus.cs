namespace CineCraft.Api.Models;

public class SolicitudStatus
{
    public int Id { get; set; }

    public required string Descripcion { get; set; }

    // Navigation
    public ICollection<Solicitud> Solicitudes { get; set; } = [];
}

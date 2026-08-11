namespace CineCraft.Api.Models;

public class Rol
{
    public int Id { get; set; }

    public required string Nombre { get; set; }

    public string? Descripcion { get; set; }

    // Navigation
    public ICollection<Usuario> Usuarios { get; set; } = [];
}

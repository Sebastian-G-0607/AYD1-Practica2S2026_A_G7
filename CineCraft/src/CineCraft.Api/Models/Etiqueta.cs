namespace CineCraft.Api.Models;

public class Etiqueta
{
    public int Id { get; set; }

    public int UsuarioId { get; set; }

    public required string Descripcion { get; set; }

    // Navigation
    public Usuario? Usuario { get; set; }

    public ICollection<Resenia> Resenias { get; set; } = [];
}

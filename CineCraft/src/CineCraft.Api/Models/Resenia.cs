namespace CineCraft.Api.Models;

public class Resenia
{
    public int Id { get; set; }

    public int UsuarioAutorId { get; set; }

    public required string TituloPelicula { get; set; }

    public int Calificacion { get; set; }

    public string? Comentario { get; set; }

    public int EtiquetaId { get; set; }

    public bool Destacada { get; set; }

    public bool Archivada { get; set; }

    public bool Deleted { get; set; }

    // Navigation
    public Usuario? UsuarioAutor { get; set; }

    public Etiqueta? Etiqueta { get; set; }

    public ICollection<CompartirReseniaUsuario> Compartidos { get; set; } = [];
}

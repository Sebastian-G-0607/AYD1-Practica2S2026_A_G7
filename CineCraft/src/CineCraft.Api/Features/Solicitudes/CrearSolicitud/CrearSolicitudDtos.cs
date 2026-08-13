using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CineCraft.Api.Features.Solicitudes.CrearSolicitud;

public record CrearSolicitudRequestDto(
    [property: JsonPropertyName("nombre")]
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(150, ErrorMessage = "El nombre no puede tener más de 150 caracteres.")]
    string Nombre,

    [property: JsonPropertyName("correo")]
    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "El correo debe tener un formato válido.")]
    [StringLength(150, ErrorMessage = "El correo no puede tener más de 150 caracteres.")]
    string Correo,

    [property: JsonPropertyName("contrasenia")]
    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [StringLength(255, ErrorMessage = "La contraseña no puede tener más de 255 caracteres.")]
    string Contrasenia,

    [property: JsonPropertyName("confirmacion_contrasenia")]
    [Required(ErrorMessage = "La confirmación de la contraseña es obligatoria.")]
    string ConfirmacionContrasenia
);

public record SolicitudResponseDto(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("nombre")] string Nombre,
    [property: JsonPropertyName("correo")] string Correo,
    [property: JsonPropertyName("status_id")] int StatusId,
    [property: JsonPropertyName("status")] string Status,
    [property: JsonPropertyName("fecha_solicitud")] DateTime FechaSolicitud
);

using System.Text.Json.Serialization;

namespace CineCraft.Api.Features.Admin.Solicitudes;

public record AdminSolicitudDto(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("nombre")] string Nombre,
    [property: JsonPropertyName("correo")] string Correo,
    [property: JsonPropertyName("fecha_solicitud")] DateTime FechaSolicitud,
    [property: JsonPropertyName("status")] string Status,
    [property: JsonPropertyName("motivo_rechazo")] string? MotivoRechazo
);

public record ProcesarSolicitudRequestDto(
    [property: JsonPropertyName("solicitud_id")] int SolicitudId,
    [property: JsonPropertyName("aprobar")] bool Aprobar,
    [property: JsonPropertyName("motivo_rechazo")] string? MotivoRechazo
);

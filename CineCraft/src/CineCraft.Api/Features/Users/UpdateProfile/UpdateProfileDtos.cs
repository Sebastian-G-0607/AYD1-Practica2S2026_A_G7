namespace CineCraft.Api.Features.Users.UpdateProfile;

public record UpdateProfileRequest(
    string Nombre, 
    string Correo, 
    string? ContraseniaActual, 
    string? NuevaContrasenia
);

public record UpdateProfileResponse(
    int Id,
    string Nombre,
    string Correo,
    string Mensaje
);
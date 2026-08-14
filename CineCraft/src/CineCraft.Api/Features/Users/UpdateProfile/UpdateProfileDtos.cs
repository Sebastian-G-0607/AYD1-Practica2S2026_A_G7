namespace CineCraft.Api.Features.Users.UpdateProfile;

public record UpdateProfileRequest(
    string Nombre, 
    string Usuario, 
    string Correo, 
    string? NuevaContrasenia
);
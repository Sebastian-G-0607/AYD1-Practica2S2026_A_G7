namespace CineCraft.Api.Features.Users.GetProfile;

public record UserProfileDto(
    int Id,
    string Nombre,
    string Correo,
    string Rol,
    int TotalResenias,
    int TotalCompartidas
);

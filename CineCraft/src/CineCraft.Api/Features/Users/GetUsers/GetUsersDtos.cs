namespace CineCraft.Api.Features.Users.GetUsers;

public record UserDto(
    int Id, 
    string Nombre, 
    string Correo,
    string Rol
);

using CineCraft.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace CineCraft.Api.Features.Users.UpdateProfile;

public static class UpdateProfileEndpoint
{
    public static void MapUpdateProfile(this IEndpointRouteBuilder app)
    {
        // PUT: /api/users/{id}
        app.MapPut("/{id:int}", async (int id, UpdateProfileRequest request, CineCraftContext dbContext) =>
        {
            var usuario = await dbContext.Usuarios.FindAsync(id);

            if (usuario is null)
            {
                return Results.NotFound(new { Mensaje = "Usuario no encontrado." });
            }

            // 1. Actualizamos los campos que sí existen en el diagrama ER
            usuario.Nombre = request.Nombre;
            usuario.Correo = request.Correo;

            // 2. Si el frontend envió una nueva contraseña (no es null ni vacía), la actualizamos
            if (!string.IsNullOrWhiteSpace(request.NuevaContrasenia))
            {
                usuario.Contrasenia = request.NuevaContrasenia;
            }

            await dbContext.SaveChangesAsync();

            return Results.Ok(new { Mensaje = "Perfil actualizado con éxito." });
        });
    }
}
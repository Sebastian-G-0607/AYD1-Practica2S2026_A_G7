using CineCraft.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace CineCraft.Api.Features.Users.UpdateProfile;

public static class UpdateProfileEndpoint
{
    public static void MapUpdateProfile(this IEndpointRouteBuilder app)
    {
        // PUT: /api/users/actualizar/{id} and /api/users/{id}
        var handler = async (int id, UpdateProfileRequest request, CineCraftContext dbContext) =>
        {
            if (string.IsNullOrWhiteSpace(request.Nombre))
            {
                return Results.BadRequest(new { Mensaje = "El nombre no puede estar vacío." });
            }

            if (string.IsNullOrWhiteSpace(request.Correo))
            {
                return Results.BadRequest(new { Mensaje = "El correo no puede estar vacío." });
            }

            var usuario = await dbContext.Usuarios.FindAsync(id);

            if (usuario is null)
            {
                return Results.NotFound(new { Mensaje = "Usuario no encontrado." });
            }

            var correoNormalizado = request.Correo.Trim().ToLowerInvariant();
            var correoEnUso = await dbContext.Usuarios
                .AnyAsync(u => u.Id != id && u.Correo.ToLower() == correoNormalizado);

            if (correoEnUso)
            {
                return Results.BadRequest(new { Mensaje = "El correo electrónico ya se encuentra en uso por otro usuario." });
            }

            usuario.Nombre = request.Nombre.Trim();
            usuario.Correo = request.Correo.Trim();

            if (!string.IsNullOrWhiteSpace(request.NuevaContrasenia))
            {
                if (string.IsNullOrWhiteSpace(request.ContraseniaActual))
                {
                    return Results.BadRequest(new { Mensaje = "Debes ingresar tu contraseña actual para establecer una nueva." });
                }

                if (!BCrypt.Net.BCrypt.Verify(request.ContraseniaActual, usuario.Contrasenia))
                {
                    return Results.BadRequest(new { Mensaje = "La contraseña actual es incorrecta." });
                }

                usuario.Contrasenia = BCrypt.Net.BCrypt.HashPassword(request.NuevaContrasenia);
            }

            await dbContext.SaveChangesAsync();

            return Results.Ok(new UpdateProfileResponse(
                usuario.Id,
                usuario.Nombre,
                usuario.Correo,
                "Perfil actualizado con éxito."
            ));
        };

        app.MapPut("/actualizar/{id:int}", handler)
           .Produces<UpdateProfileResponse>(StatusCodes.Status200OK)
           .Produces(StatusCodes.Status400BadRequest)
           .Produces(StatusCodes.Status404NotFound);

        app.MapPut("/{id:int}", handler)
           .Produces<UpdateProfileResponse>(StatusCodes.Status200OK)
           .Produces(StatusCodes.Status400BadRequest)
           .Produces(StatusCodes.Status404NotFound);
    }
}
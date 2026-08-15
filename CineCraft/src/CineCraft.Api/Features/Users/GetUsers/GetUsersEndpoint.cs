using System.IdentityModel.Tokens.Jwt;
using CineCraft.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace CineCraft.Api.Features.Users.GetUsers;

public static class GetUsersEndpoint
{
    public static void MapGetUsers(this IEndpointRouteBuilder app)
    {
        // GET: /api/users?search={search}&excludeUserId={excludeUserId}
        app.MapGet("/", async (
            string? search,
            int? excludeUserId,
            HttpContext httpContext,
            CineCraftContext dbContext) =>
        {
            var currentUserId = excludeUserId;
            if (!currentUserId.HasValue)
            {
                var userIdClaim = httpContext.User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
                if (int.TryParse(userIdClaim, out var parsedId))
                {
                    currentUserId = parsedId;
                }
            }

            var query = dbContext.Usuarios
                .Include(u => u.Rol)
                .AsNoTracking()
                .AsQueryable();

            if (currentUserId.HasValue)
            {
                query = query.Where(u => u.Id != currentUserId.Value);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim().ToLower();
                query = query.Where(u =>
                    u.Nombre.ToLower().Contains(term) ||
                    u.Correo.ToLower().Contains(term));
            }

            var users = await query
                .OrderBy(u => u.Nombre)
                .Select(u => new UserDto(
                    u.Id,
                    u.Nombre,
                    u.Correo,
                    u.Rol != null ? u.Rol.Nombre : "estandar"
                ))
                .ToListAsync();

            return Results.Ok(users);
        })
        .Produces<List<UserDto>>(StatusCodes.Status200OK);
    }
}

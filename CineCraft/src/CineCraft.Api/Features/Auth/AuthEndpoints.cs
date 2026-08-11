using CineCraft.Api.Features.Auth.Login;

namespace CineCraft.Api.Features.Auth;

public static class AuthEndpoints
{
    public static void MapAuth(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api");

        group.MapLogin();
    }
}

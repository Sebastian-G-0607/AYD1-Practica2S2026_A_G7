using CineCraft.Api.Features.Users.UpdateProfile;

namespace CineCraft.Api.Features.Users;

public static class UsersEndpoints
{
    public static void MapUsers(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/users");

        group.MapUpdateProfile();
    }
}
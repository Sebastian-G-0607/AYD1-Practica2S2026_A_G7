using CineCraft.Api.Features.Resenias.GetResenias;

namespace CineCraft.Api.Features.Resenias;

public static class ReseniasEndpoints
{
    public static void MapResenias(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/resenias");

        group.MapGetResenias();
    }
}
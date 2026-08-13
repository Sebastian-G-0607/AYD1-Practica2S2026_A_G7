using CineCraft.Api.Features.Resenias.CreateResenia;
using CineCraft.Api.Features.Resenias.GetResenias;
using CineCraft.Api.Features.Resenias.UpdateResenia;
using CineCraft.Api.Features.Resenias.DeleteResenia;

namespace CineCraft.Api.Features.Resenias;

public static class ReseniasEndpoints
{
    public static void MapResenias(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/resenias");

        group.MapGetResenias();
        group.MapCreateResenia();
        group.MapUpdateResenia();
        group.MapDeleteResenia();
    }
}
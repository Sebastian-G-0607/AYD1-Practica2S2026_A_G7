using CineCraft.Api.Features.Resenias.CreateResenia;
using CineCraft.Api.Features.Resenias.GetResenias;
using CineCraft.Api.Features.Resenias.UpdateResenia;
using CineCraft.Api.Features.Resenias.DeleteResenia;
using CineCraft.Api.Features.Resenias.GetReseniasArchivadas;
using CineCraft.Api.Features.Resenias.ToggleArchivar;
using CineCraft.Api.Features.Resenias.ToggleDestacar;

namespace CineCraft.Api.Features.Resenias;

public static class ReseniasEndpoints
{
    public static void MapResenias(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/resenias");

        group.MapGetResenias();
        group.MapGetReseniasArchivadas();
        group.MapCreateResenia();
        group.MapUpdateResenia();
        group.MapDeleteResenia();
        group.MapToggleDestacar();
        group.MapToggleArchivar();
    }
}
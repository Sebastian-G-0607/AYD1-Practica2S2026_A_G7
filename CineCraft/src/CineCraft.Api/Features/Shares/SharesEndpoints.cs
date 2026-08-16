using CineCraft.Api.Features.Shares.ShareReview;
using CineCraft.Api.Features.Shares.GetSharedWithMe;
using CineCraft.Api.Features.Shares.GetMyShares;
using CineCraft.Api.Features.Shares.UnshareReview;

namespace CineCraft.Api.Features.Shares;

public static class SharesEndpoints
{
    public static void MapShares(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/shares");

        group.MapShareReview();
        group.MapGetSharedWithMe(); 
        group.MapGetMyShares();     
        group.MapUnshareReview();
    }
}
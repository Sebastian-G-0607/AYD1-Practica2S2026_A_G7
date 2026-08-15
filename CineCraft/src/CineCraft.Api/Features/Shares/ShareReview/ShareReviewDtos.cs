namespace CineCraft.Api.Features.Shares.ShareReview;

public record ShareReviewRequest(
    int ReseniaId, 
    int? UsuarioRemitenteId, 
    int UsuarioDestinatarioId
);

public record ShareReviewBatchRequest(
    int ReseniaId,
    int? UsuarioRemitenteId,
    List<int> DestinatariosIds
);
namespace CineCraft.Api.Features.Shares.ShareReview;

// Objeto que recibiremos desde el Frontend
public record ShareReviewRequest(int ReseniaId, int UsuarioRemitenteId, int UsuarioDestinatarioId);
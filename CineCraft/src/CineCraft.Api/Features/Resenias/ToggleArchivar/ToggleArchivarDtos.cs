
namespace CineCraft.Api.Features.Resenias.ToggleArchivar;

public record ToggleArchivarResponse(
    int Id,
    bool Archivada,
    string Message
);
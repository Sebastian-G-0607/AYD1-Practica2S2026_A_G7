
namespace CineCraft.Api.Features.Resenias.ToggleDestacar;

public record ToggleDestacarResponse(
    int Id,
    bool Destacada,
    string Message
);
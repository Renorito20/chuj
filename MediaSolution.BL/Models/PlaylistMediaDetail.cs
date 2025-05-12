namespace MediaSolution.BL.Models;

public record PlaylistMediaDetail : ModelBase
{
    public required int Order { get; init; }
    public required Guid MediaId { get; init; }
    public required Guid PlaylistId { get; init; }

    public static PlaylistMediaDetail Empty => new()
    {
        Id = Guid.Empty,
        Order = 0,
        MediaId = Guid.Empty,
        PlaylistId = Guid.Empty,
    };
};
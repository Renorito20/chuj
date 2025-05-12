namespace MediaSolution.BL.Models;

public record PlaylistMediaList : ModelBase
{
    public required int Order { get; init; }
    public required Guid MediaId { get; init; }

    public static PlaylistMediaList Empty => new()
    {
        Id = Guid.Empty,
        Order = 0,
        MediaId = Guid.Empty,
    };
};
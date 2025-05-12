namespace MediaSolution.DAL.Entities;

public record PlaylistMediaEntity : IEntity
{
    public required Guid Id { get; set; }
    public required int Order { get; set; }

    public required Guid PlaylistId { get; set; }
    public PlaylistEntity? Playlist { get; set; }

    public required Guid MediaId { get; set; }
    public MediaEntity? Media { get; set; }
}
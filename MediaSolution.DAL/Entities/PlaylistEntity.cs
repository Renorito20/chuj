namespace MediaSolution.DAL.Entities;

public record PlaylistEntity : IEntity
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public bool Favorite { get; set; }
    public required string CoverImage { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ICollection<PlaylistMediaEntity> Media { get; set; } = new List<PlaylistMediaEntity>();
}
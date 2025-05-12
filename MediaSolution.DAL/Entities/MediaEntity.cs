namespace MediaSolution.DAL.Entities;

public record MediaEntity : IEntity
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required TimeSpan Duration { get; set; }
    public required long SizeInBytes { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public string? Authors { get; set; }
    public string? Genres { get; set; }
    public required string Path { get; set; }
    
}
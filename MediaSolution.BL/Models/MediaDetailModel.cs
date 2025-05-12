namespace MediaSolution.BL.Models;

public record MediaDetailModel : ModelBase
{
    public required string Name { get; set; }
    public required TimeSpan Duration { get; set; }
    public required long SizeInBytes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string? Authors { get; set; }
    public string? Genres { get; set; }
    public required string Path { get; set; }
    
    public static MediaDetailModel Empty => new()
    {
        Name = String.Empty,
        Duration = TimeSpan.Zero,
        SizeInBytes = 0,
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow,
        Authors = String.Empty,
        Genres = String.Empty,
        Path = String.Empty,
    };
}
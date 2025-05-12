namespace MediaSolution.BL.Models;

public record PlaylistDetailModel : ModelBase
{
    public required string Name { get; set; }
    public bool Favorite { get; set; }
    public required string CoverImage { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public static PlaylistDetailModel Empty => new()
    {
        Id = Guid.Empty,
        Name = string.Empty,
        Favorite = false,
        CoverImage = "default.jpg",
        Description = string.Empty,
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    };
}
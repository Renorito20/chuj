namespace MediaSolution.BL.Models;

public record MediaListModel : ModelBase
{
    public required string Name { get; set; }
    public required TimeSpan Duration { get; set; }
    public string? Authors { get; set; }
    public string? Genres { get; set; }

    public static MediaListModel Empty => new()
    {
        Id = Guid.Empty,
        Name = string.Empty,
        Duration = TimeSpan.Zero,
        Authors = string.Empty,
        Genres = string.Empty
    };
}
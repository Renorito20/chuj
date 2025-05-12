using System.Collections.ObjectModel;
 
namespace MediaSolution.BL.Models;

public record PlaylistListModel : ModelBase
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public bool Favorite { get; set; }
    public required string CoverImage { get; set; }
    public required List<Guid> MediaIds { get; set; }
    public static PlaylistListModel Empty => new()
    {
        Id = Guid.Empty,
        Favorite = false,
        Name = string.Empty,
        Description = string.Empty,
        CoverImage = "default.jpg",
        MediaIds = new List<Guid>()
    };
}
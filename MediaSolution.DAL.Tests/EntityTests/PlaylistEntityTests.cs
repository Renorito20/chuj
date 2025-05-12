using MediaSolution.DAL.Entities;

namespace MediaSolution.DAL.Tests.EntityTests;

public class PlaylistEntityTests
{
    [Fact]
    public void TestPlaylist_ShouldInitialize()
    {
        // arrange  
        var newPlaylist = new PlaylistEntity
        {
            Id = Guid.NewGuid(),
            Name = "My New Playlist",
            CreatedAt = DateTime.UtcNow,
            CoverImage = "default.png",
        };
        
        // assert
        Assert.NotNull(newPlaylist);
        Assert.NotEqual(Guid.Empty, newPlaylist.Id);
        Assert.False(newPlaylist.Favorite);
        
    }
    
}
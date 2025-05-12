using MediaSolution.BL.Facades;
using MediaSolution.BL.Mappers;
using MediaSolution.BL.Models;
using MediaSolution.Common.Tests.Seeds;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace MediaSolution.BL.Tests;

public class PlaylistFacadeTests : FacadeTestsBase
{
    private readonly PlaylistFacade _playlistFacade;

    public PlaylistFacadeTests(ITestOutputHelper output) : base(output)
    {
        _playlistFacade = new PlaylistFacade(UnitOfWorkFactory, PlaylistModelMapper);
    }

    [Fact]
    public async Task GetAll_ReturnsSeededPlaylists()
    {
        var result = await _playlistFacade.GetAsync();
        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task Save_NewPlaylist_WorksCorrectly()
    {
        var model = new PlaylistDetailModel
        {
            Name = "My Playlist one",
            Description = "songs I like",
            Favorite = false,
            CoverImage = "path/img.png"
        };

        var saved = await _playlistFacade.SaveAsync(model);

        Assert.NotEqual(Guid.Empty, saved.Id);
        Assert.Equal("My Playlist one", saved.Name);
    }
    [Fact]
    public async Task Create_ValidPlaylist_CreatesSuccessfully()
    {
        var model = new PlaylistDetailModel
        {
            Id = Guid.Empty,
            Name = "Test Playlist",
            Description = "Just some songs",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Favorite = false,
            CoverImage = "/path/img.png"
        };

        var saved = await _playlistFacade.SaveAsync(model);
        Assert.NotEqual(Guid.Empty, saved.Id);
        Assert.Equal(model.Name, saved.Name);
    }

    [Fact]
    public async Task Update_SeededPlaylist_UpdatesName()
    {
        var model = PlaylistModelMapper.MapToDetailModel(PlaylistSeeds.PlaylistUpdate);
        model.Name = "New name";
        model.Description = "Updated description";


        await _playlistFacade.SaveAsync(model);
        var fromDb = await _playlistFacade.GetAsync(model.Id);

        Assert.Equal("New name", fromDb!.Name);
        Assert.Equal("Updated description", fromDb!.Description);
    }

    [Fact]
    public async Task Delete_SeededPlaylist_RemovesIt()
    {
        await _playlistFacade.DeleteAsync(PlaylistSeeds.PlaylistDelete.Id);

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var exists = await dbx.PlaylistEntities.AnyAsync(p => p.Id == PlaylistSeeds.PlaylistDelete.Id);
        Assert.False(exists);
    }
}

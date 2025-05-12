using MediaSolution.BL.Facades;
using MediaSolution.BL.Mappers;
using MediaSolution.BL.Models;
using MediaSolution.Common.Tests.Seeds;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace MediaSolution.BL.Tests;

public class PlaylistMediaFacadeTests : FacadeTestsBase
{
    private readonly PlaylistMediaFacade _playlistMediaFacade;

    public PlaylistMediaFacadeTests(ITestOutputHelper output) : base(output)
    {
        _playlistMediaFacade = new PlaylistMediaFacade(UnitOfWorkFactory, PlaylistMediaModelMapper);
    }

    [Fact]
    public async Task Save_NewPlaylistMedia_WorksCorrectly()
    {
        var model = new PlaylistMediaDetail
        {
            PlaylistId = Guid.Empty,
            Order = 1,
            MediaId = MediaSeeds.Layla.Id
        };

        await _playlistMediaFacade.SaveAsync(model, PlaylistSeeds.EricClaptonPlaylist.Id);

        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();

        var exists = await dbxAssert.PlaylistMediaEntities
            .AnyAsync(pm => pm.MediaId == MediaSeeds.Layla.Id
                         && pm.PlaylistId == PlaylistSeeds.EricClaptonPlaylist.Id);

        Assert.True(exists);
    }

    [Fact]
    public async Task Delete_PlaylistMedia_Works()
    {
        var all = await _playlistMediaFacade.GetAsync();
        var toDelete = all.FirstOrDefault();
        Assert.NotNull(toDelete);

        await _playlistMediaFacade.DeleteAsync(toDelete!.Id);

        var afterDelete = await _playlistMediaFacade.GetAsync();
        Assert.DoesNotContain(afterDelete, x => x.Id == toDelete.Id);
    }
    [Fact]
    public async Task Save_ValidPlaylistMedia_AddsIt()
    {
        var model = new PlaylistMediaDetail
        {
            PlaylistId = PlaylistSeeds.EricClaptonPlaylist.Id,
            MediaId = MediaSeeds.MediaUpdate.Id,
            Order = 1
        };

        await _playlistMediaFacade.SaveAsync(model, PlaylistSeeds.EricClaptonPlaylist.Id);

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var exists = await dbx.PlaylistMediaEntities.AnyAsync(
            pm => pm.PlaylistId == model.PlaylistId && pm.MediaId == model.MediaId);

        Assert.True(exists);
    }

    [Fact]
    public async Task Delete_SeededPlaylistMedia_RemovesIt()
    {
        await using var dbxPre = await DbContextFactory.CreateDbContextAsync();
        var existsBefore = await dbxPre.PlaylistMediaEntities.AnyAsync(pm => pm.Id == PlaylistMediaSeeds.PlaylistMediaDelete.Id);
        Assert.True(existsBefore);

        await _playlistMediaFacade.DeleteAsync(PlaylistMediaSeeds.PlaylistMediaDelete.Id);

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var exists = await dbx.PlaylistMediaEntities.AnyAsync(pm => pm.Id == PlaylistMediaSeeds.PlaylistMediaDelete.Id);

        Assert.False(exists);
    }
}

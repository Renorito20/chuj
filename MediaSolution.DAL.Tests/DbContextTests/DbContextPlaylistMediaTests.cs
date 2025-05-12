using MediaSolution.Common.Tests;
using MediaSolution.Common.Tests.Seeds;
using MediaSolution.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace MediaSolution.DAL.Tests.DbContextTests;

public class DbContextPlaylistMediaTests(ITestOutputHelper output) : DbContextTestsBase(output)
{
    [Fact]
    public async Task GetAll_PlaylistMedia_ForPlaylist()
    {
        // Act
        var playlistMediaEntities = await MediaSolutionDbContextSUT.PlaylistMediaEntities
            .Where(pm => pm.PlaylistId == PlaylistSeeds.EricClaptonPlaylist.Id)
            .ToArrayAsync();

        // Assert
        Assert.Contains(PlaylistMediaSeeds.PlaylistMediaLayla with { Playlist = null!, Media = null! }, playlistMediaEntities);
        Assert.Contains(PlaylistMediaSeeds.PlaylistMediaTearsInHeaven with { Playlist = null!, Media = null! }, playlistMediaEntities);
    }
    
    [Fact]
    public async Task GetAll_PlaylistMedia_IncludingMedia_ForPlaylist()
    {
        // Act
        var playlistMediaEntities = await MediaSolutionDbContextSUT.PlaylistMediaEntities
            .Where(pm => pm.PlaylistId == PlaylistMediaSeeds.PlaylistMediaLayla.PlaylistId)
            .Include(pm => pm.Media)  // Include related MediaEntity
            .ToArrayAsync();

        // Assert
        Assert.Contains(playlistMediaEntities, pm => pm.Id == PlaylistMediaSeeds.PlaylistMediaLayla.Id);
        Assert.Contains(playlistMediaEntities, pm => pm.Id == PlaylistMediaSeeds.PlaylistMediaTearsInHeaven.Id);
    }


    [Fact]
    public async Task Update_PlaylistMedia_Persisted()
    {
        // Arrange
        var baseEntity = PlaylistMediaSeeds.PlaylistMediaUpdate;
        var entity =
            baseEntity with
            {
                Order = baseEntity.Order + 1,
                Playlist = null!,  // Set Playlist to null to avoid navigation property issues
                Media = null!      // Set Media to null to avoid navigation property issues
            };

        // Act
        MediaSolutionDbContextSUT.PlaylistMediaEntities.Update(entity);
        await MediaSolutionDbContextSUT.SaveChangesAsync();

        // Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.PlaylistMediaEntities.SingleAsync(pm => pm.Id == entity.Id);
        Assert.Equal(entity, actualEntity);
    }
    
    [Fact]
    public async Task Delete_PlaylistMedia_Deleted()
    {
        // Arrange
        var baseEntity = PlaylistMediaSeeds.PlaylistMediaDelete;

        // Act
        MediaSolutionDbContextSUT.PlaylistMediaEntities.Remove(baseEntity);
        await MediaSolutionDbContextSUT.SaveChangesAsync();

        // Assert
        Assert.False(await MediaSolutionDbContextSUT.PlaylistMediaEntities.AnyAsync(pm => pm.Id == baseEntity.Id));
    }
    
    [Fact]
    public async Task DeleteById_PlaylistMedia_Deleted()
    {
        // Arrange
        var baseEntity = PlaylistMediaSeeds.PlaylistMediaDelete;

        // Act
        MediaSolutionDbContextSUT.Remove(
            MediaSolutionDbContextSUT.PlaylistMediaEntities.Single(pm => pm.Id == baseEntity.Id));
        await MediaSolutionDbContextSUT.SaveChangesAsync();

        // Assert
        Assert.False(await MediaSolutionDbContextSUT.PlaylistMediaEntities.AnyAsync(pm => pm.Id == baseEntity.Id));
    }


}
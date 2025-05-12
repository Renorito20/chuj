using MediaSolution.Common.Tests;
using MediaSolution.Common.Tests.Seeds;
using MediaSolution.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace MediaSolution.DAL.Tests.DbContextTests;

public class DbContextPlaylistTests(ITestOutputHelper output) : DbContextTestsBase(output)
{
    [Fact]
    public async Task AddNew_PlaylistWithoutMedia_Persisted()
    {
        //Arrange
        var entity = PlaylistSeeds.EmptyPlaylist with
        {
            Name = "Eric Clapton",
            Description = "test"
        };

        //Act
        MediaSolutionDbContextSUT.PlaylistEntities.Add(entity);
        await MediaSolutionDbContextSUT.SaveChangesAsync();

        //Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.PlaylistEntities
            .SingleAsync(i => i.Id == entity.Id);
        DeepAssert.Equal(entity, actualEntity);
    }

    [Fact]
    public async Task AddNew_PlaylistWithMedia_Persisted()
    {
        //Arrange
        var entity = PlaylistSeeds.EmptyPlaylist with
        {
            Name = "Pop",
            Description = "Description 1",
            Media = new List<PlaylistMediaEntity>
            {
                PlaylistMediaSeeds.EmptyPlaylistMedia with
                {
                    Order = 1,
                    Media = MediaSeeds.EmptyMedia with
                    {
                        Name = "Layla",
                        Duration = TimeSpan.FromMinutes(3)
                    }
                },
                PlaylistMediaSeeds.EmptyPlaylistMedia with
                {
                    Order = 2,
                    Media = MediaSeeds.EmptyMedia with
                    {
                        Name = "tears in heaven",
                        Duration = TimeSpan.FromMinutes(3)
                    }
                },
            }
        };

        //Act
        MediaSolutionDbContextSUT.PlaylistEntities.Add(entity);
        await MediaSolutionDbContextSUT.SaveChangesAsync();

        //Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.PlaylistEntities
            .Include(i => i.Media)
            .ThenInclude(i => i.Media)
            .SingleAsync(i => i.Id == entity.Id);
        DeepAssert.Equal(entity, actualEntity);
    }

    [Fact]
    public async Task AddNewPlaylistWithJustPlaylistMediaPersisted()
    {
        //Arrange
        var entity = PlaylistSeeds.EmptyPlaylist with
        {
            Name = "Pop",
            Description = "Description 1",
            Media = new List<PlaylistMediaEntity>
            {
                PlaylistMediaSeeds.EmptyPlaylistMedia with
                {
                    Order = 1,
                    MediaId = MediaSeeds.TearsInHeaven.Id
                },
                PlaylistMediaSeeds.EmptyPlaylistMedia with
                {
                    Order = 2,
                    MediaId = MediaSeeds.BeforeYouAccuseMe.Id
                },
            }
        };

        //Act
        MediaSolutionDbContextSUT.PlaylistEntities.Add(entity);
        await MediaSolutionDbContextSUT.SaveChangesAsync();

        //Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.PlaylistEntities
            .Include(i => i.Media)
            .SingleAsync(i => i.Id == entity.Id);
        DeepAssert.Equal(entity, actualEntity);
    }

    [Fact]
    public async Task GetAll_Playlists_ContainsSeededMedia()
    {
        //Act
        var entities = await MediaSolutionDbContextSUT.PlaylistEntities.ToListAsync();

        //Assert
        DeepAssert.Contains(PlaylistSeeds.EricClaptonPlaylist, entities,
            nameof(PlaylistEntity.Media));
    }

    [Fact]
    public async Task GetById_Playlist()
    {
        //Act
        var entity = await MediaSolutionDbContextSUT.PlaylistEntities
            .SingleAsync(i => i.Id == PlaylistSeeds.EricClaptonPlaylist.Id);

        //Assert
        DeepAssert.Equal(PlaylistSeeds.EricClaptonPlaylist with { Media = Array.Empty<PlaylistMediaEntity>() }, entity);
    }

    [Fact]
    public async Task GetByIdIncludingMediaPlaylist()
    {
        //Act
        var entity = await MediaSolutionDbContextSUT.PlaylistEntities
            .Include(i => i.Media)
            .ThenInclude(i => i.Media)
            .SingleAsync(i => i.Id == PlaylistSeeds.EricClaptonPlaylist.Id);

        //Assert
        DeepAssert.Equal(PlaylistSeeds.EricClaptonPlaylist, entity);
    }

    [Fact]
    public async Task Update_Playlist_Persisted()
    {
        //Arrange
        var baseEntity = PlaylistSeeds.PlaylistUpdate;
        var entity =
            baseEntity with
            {
                Name = baseEntity.Name + "Updated",
                Description = baseEntity.Description + "Updated",
                Favorite = false,
                CoverImage = baseEntity.Description + "Updated"
            };

        //Act
        MediaSolutionDbContextSUT.PlaylistEntities.Update(entity);
        await MediaSolutionDbContextSUT.SaveChangesAsync();

        //Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.PlaylistEntities.SingleAsync(i => i.Id == entity.Id);
        DeepAssert.Equal(entity, actualEntity);
    }

    [Fact]
    public async Task Delete_PlaylistWithoutMedia_Deleted()
    {
        //Arrange
        var baseEntity = PlaylistSeeds.PlaylistDelete;

        //Act
        MediaSolutionDbContextSUT.PlaylistEntities.Remove(baseEntity);
        await MediaSolutionDbContextSUT.SaveChangesAsync();

        //Assert
        Assert.False(await MediaSolutionDbContextSUT.PlaylistEntities.AnyAsync(i => i.Id == baseEntity.Id));
    }

    [Fact]
    public async Task DeleteById_PlaylistWithoutMedia_Deleted()
    {
        //Arrange
        var baseEntity = PlaylistSeeds.PlaylistDelete;

        //Act
        MediaSolutionDbContextSUT.Remove(
            MediaSolutionDbContextSUT.PlaylistEntities.Single(i => i.Id == baseEntity.Id));
        await MediaSolutionDbContextSUT.SaveChangesAsync();

        //Assert
        Assert.False(await MediaSolutionDbContextSUT.PlaylistEntities.AnyAsync(i => i.Id == baseEntity.Id));
    }

    [Fact]
    public async Task GetPlaylist_CalculateTotalDuration_ReturnsCorrectSum()
    {
        // Arrange
        var entity = PlaylistSeeds.EmptyPlaylist with
        {
            Name = "Pop",
            Description = "Description 1",
            Media = new List<PlaylistMediaEntity>
            {
                PlaylistMediaSeeds.EmptyPlaylistMedia with
                {
                    Order = 1,
                    MediaId = MediaSeeds.TearsInHeaven.Id
                },
                PlaylistMediaSeeds.EmptyPlaylistMedia with
                {
                    Order = 2,
                    MediaId = MediaSeeds.BeforeYouAccuseMe.Id
                },
            }
        };
        var expectedDuration = TimeSpan.FromMinutes(9);

        //Act
        MediaSolutionDbContextSUT.PlaylistEntities.Add(entity);
        await MediaSolutionDbContextSUT.SaveChangesAsync();

        var playlist = await MediaSolutionDbContextSUT.PlaylistEntities
            .Include(p => p.Media)
            .ThenInclude(pm => pm.Media)
            .SingleAsync(p => p.Id == entity.Id);

        // Then calculate the total duration
        var totalDuration = playlist.Media.Sum(pm => pm.Media.Duration.Ticks);
        var totalDurationTimeSpan = TimeSpan.FromTicks(totalDuration);

        // Assert
        Assert.NotEqual(TimeSpan.Zero, totalDurationTimeSpan);
        Assert.Equal(expectedDuration, totalDurationTimeSpan);
    }
}
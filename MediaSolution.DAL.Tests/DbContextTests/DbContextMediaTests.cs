using MediaSolution.Common.Tests;
using MediaSolution.Common.Tests.Seeds;
using MediaSolution.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace MediaSolution.DAL.Tests.DbContextTests;

public class DbContextMediaTests(ITestOutputHelper output) : DbContextTestsBase(output)
{
    [Fact]
    public async Task AddNew_Media_Persisted()
    {
        //Arrange
        MediaEntity entity = MediaSeeds.EmptyMedia with
        {
            Id = Guid.Parse("C5DE45D7-64A0-4E8D-AC7F-BF5CFDFB0EFC"),
            Name = "Wonderful Tonight",
            Duration = TimeSpan.FromMinutes(3.5),
            SizeInBytes = 4500000,
            Authors = "Eric Clapton",
            Genres = "Rock, Blues",
            Path = "/media/wonderful_tonight.mp3",
        };

        //Act
        MediaSolutionDbContextSUT.MediaEntities.Add(entity);
        await MediaSolutionDbContextSUT.SaveChangesAsync();

        //Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.MediaEntities.SingleAsync(i => i.Id == entity.Id);

        Assert.Equal(entity, actualEntity);
    }

    [Fact]
    public async Task GetAll_Media_ContainsSeededLayla()
    {
        // Act
        var entities = await MediaSolutionDbContextSUT.MediaEntities.ToArrayAsync();

        // Assert
        Assert.Contains(MediaSeeds.Layla, entities);
    }
    
    [Fact]
    public async Task GetById_Media_LaylaRetrieved()
    {
        //Act
        var entity = await MediaSolutionDbContextSUT.MediaEntities
            .SingleAsync(i=>i.Id == MediaSeeds.Layla.Id);

        //Assert
        Assert.Equal(MediaSeeds.Layla, entity);
    }
    
    [Fact]
    public async Task Update_Media_Persisted()
    {
        //Arrange
        var baseEntity = MediaSeeds.MediaUpdate;
        var entity = baseEntity with
        {
            Name = baseEntity.Name + " Updated",
            Authors = baseEntity.Authors+ " Updated",
            SizeInBytes = baseEntity.SizeInBytes+5,
        };
        
        //Act
        MediaSolutionDbContextSUT.MediaEntities.Update(entity);
        await MediaSolutionDbContextSUT.SaveChangesAsync();

        //Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.MediaEntities.SingleAsync(i => i.Id == entity.Id);
        Assert.Equivalent(entity, actualEntity);
    }
    [Fact]
    public async Task Delete_Media_MediaDeleted()
    {
        //Arrange
        var entityBase = MediaSeeds.MediaDelete;

        //Act
        MediaSolutionDbContextSUT.MediaEntities.Remove(entityBase);
        await MediaSolutionDbContextSUT.SaveChangesAsync();

        //Assert
        Assert.False(await MediaSolutionDbContextSUT.MediaEntities.AnyAsync(i => i.Id == entityBase.Id));
    }
    
    [Fact]
    public async Task DeleteById_Media_MediaDeleted()
    {
        //Arrange
        var entityBase = MediaSeeds.MediaDelete;

        //Act
        MediaSolutionDbContextSUT.Remove(
            MediaSolutionDbContextSUT.MediaEntities.Single(i => i.Id == entityBase.Id));
        await MediaSolutionDbContextSUT.SaveChangesAsync();

        //Assert
        Assert.False(await MediaSolutionDbContextSUT.MediaEntities.AnyAsync(i => i.Id == entityBase.Id));
    }
    
    [Fact]
    public async Task Delete_MediaUsedInPlaylist_Throws()
    {
        //Arrange
        var entityBase =  MediaSeeds.Layla;

        //Act & Assert
        MediaSolutionDbContextSUT.MediaEntities.Remove(entityBase);
        await Assert.ThrowsAsync<DbUpdateException>(async () => await MediaSolutionDbContextSUT.SaveChangesAsync());
    }


}
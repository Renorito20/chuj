using MediaSolution.BL.Facades;
using MediaSolution.BL.Models;
using MediaSolution.Common.Tests;
using MediaSolution.Common.Tests.Seeds;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace MediaSolution.BL.Tests;

public class MediaFacadeTests : FacadeTestsBase
{
    private readonly MediaFacade _mediaFacade;

    public MediaFacadeTests(ITestOutputHelper output) : base(output)
    {
        _mediaFacade = new MediaFacade(UnitOfWorkFactory, MediaModelMapper);
    }

    [Fact]
    public async Task Create_WithValidMedia_DoesNotThrow()
    {
        var model = new MediaDetailModel
        {
            Id = Guid.Empty,
            Name = "Test track",
            Duration = TimeSpan.FromMinutes(3),
            SizeInBytes = 1024,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Authors = "Test Author",
            Genres = "Test Genre",
            Path = "/music/test.mp3"
        };

        var result = await _mediaFacade.SaveAsync(model);
        Assert.NotEqual(Guid.Empty, result.Id);
    }

    [Fact]
    public async Task GetAll_ReturnsList_ContainingSeededMedia()
    {
        var expectedId = MediaSeeds.Layla.Id;
        var expectedName = MediaSeeds.Layla.Name;

        var result = await _mediaFacade.GetAsync();

        Assert.NotEmpty(result);

        var media = result.SingleOrDefault(m => m.Id == expectedId);
        Assert.NotNull(media);
        Assert.Equal(expectedName, media!.Name);
    }

    [Fact]
    public async Task GetById_Seeded_ReturnsCorrectData()
    {
        var expected = MediaModelMapper.MapToDetailModel(MediaSeeds.Layla);
        var id = MediaSeeds.Layla.Id;

        var media = await _mediaFacade.GetAsync(id);

        Assert.NotNull(media);
        DeepAssert.Equal(expected, media);
    }

    [Fact]
    public async Task GetById_NonExistent_ReturnsNull()
    {
        var media = await _mediaFacade.GetAsync(Guid.NewGuid());
        Assert.Null(media);
    }

    [Fact]
    public async Task Delete_SeededMedia_RemovesIt()
    {
        await _mediaFacade.DeleteAsync(MediaSeeds.MediaDelete.Id);

        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        Assert.False(await dbxAssert.MediaEntities.AnyAsync(m => m.Id == MediaSeeds.MediaDelete.Id));
    }

    [Fact]
    public async Task Update_SeededMedia_UpdatesSuccessfully()
    {
        var model = MediaModelMapper.MapToDetailModel(MediaSeeds.BeforeYouAccuseMe);
        model.Name += " (updated)";

        await _mediaFacade.SaveAsync(model);

        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        var fromDb = await dbxAssert.MediaEntities.SingleAsync(m => m.Id == model.Id);

        DeepAssert.Equal(model, MediaModelMapper.MapToDetailModel(fromDb));
    }
}

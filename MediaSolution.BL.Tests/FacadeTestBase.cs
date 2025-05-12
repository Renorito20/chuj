using MediaSolution.BL.Mappers;
using MediaSolution.DAL;
using MediaSolution.DAL.Factories;
using MediaSolution.DAL.UnitOfWork;
using MediaSolution.Common.Tests.Seeds;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace MediaSolution.BL.Tests;

public class FacadeTestsBase : IAsyncLifetime
{
    protected FacadeTestsBase(ITestOutputHelper output)
    {
        DbContextFactory = new DbContextSQLiteFactory(GetType().FullName!);

        MediaModelMapper = new MediaModelMapper();
        PlaylistModelMapper = new PlaylistModelMapper(MediaModelMapper);
        PlaylistMediaModelMapper = new PlaylistMediaModelMapper();

        UnitOfWorkFactory = new UnitOfWorkFactory(DbContextFactory);
    }

    protected IDbContextFactory<MediaSolutionDbContext> DbContextFactory { get; }
    protected UnitOfWorkFactory UnitOfWorkFactory { get; }

    protected MediaModelMapper MediaModelMapper { get; }
    protected PlaylistModelMapper PlaylistModelMapper { get; }
    protected PlaylistMediaModelMapper PlaylistMediaModelMapper { get; }
    public async Task InitializeAsync()
    {
        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();

        dbContext
            .SeedMedia()
            .SeedPlaylists()
            .SeedPlaylistMedia();

        await dbContext.SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        await using var dbContext = await DbContextFactory.CreateDbContextAsync();
        await dbContext.Database.EnsureDeletedAsync();
    }
}

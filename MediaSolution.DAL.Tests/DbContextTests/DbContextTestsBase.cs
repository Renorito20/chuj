
using MediaSolution.Common.Tests;
using MediaSolution.Common.Tests.Seeds;
using MediaSolution.DAL.Factories;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;
namespace MediaSolution.DAL.Tests.DbContextTests;


public class DbContextTestsBase : IAsyncLifetime
{
    protected DbContextTestsBase(ITestOutputHelper output)
    {
        XUnitTestOutputConverter converter = new(output);
        Console.SetOut(converter);

        DbContextFactory = new DbContextSQLiteFactory(GetType().FullName!);
        MediaSolutionDbContextSUT = DbContextFactory.CreateDbContext();
    }

    protected IDbContextFactory<MediaSolutionDbContext> DbContextFactory { get; }
    protected MediaSolutionDbContext MediaSolutionDbContextSUT { get; }


    public async Task InitializeAsync()
    {
        await MediaSolutionDbContextSUT.Database.EnsureDeletedAsync();
        await MediaSolutionDbContextSUT.Database.EnsureCreatedAsync();

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        dbx
            .SeedMedia()
            .SeedPlaylists()
            .SeedPlaylistMedia();
        await dbx.SaveChangesAsync();
    }

    public async Task DisposeAsync()
    {
        await MediaSolutionDbContextSUT.Database.EnsureDeletedAsync();
        await MediaSolutionDbContextSUT.DisposeAsync();
    }
}
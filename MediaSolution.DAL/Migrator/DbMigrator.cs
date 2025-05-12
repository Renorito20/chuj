using MediaSolution.DAL.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;


namespace MediaSolution.DAL.Migrator;

public class DbMigrator(IDbContextFactory<MediaSolutionDbContext> dbContextFactory, IOptions<DALOptions> options)
: IDbMigrator
{
    public void Migrate()
    {
        using MediaSolutionDbContext dbContext = dbContextFactory.CreateDbContext();

        if(options.Value.RecreateDatabaseEachTime)
        {
            dbContext.Database.EnsureDeleted();
        }

        // Ensures that database is created applying the latest state
        // Application of migration later on may fail
        // If you want to use migrations, you should create database by calling  dbContext.Database.Migrate() instead
        dbContext.Database.EnsureCreated();
    }

    public async Task MigrateAsync(CancellationToken cancellationToken)
    {
        await using MediaSolutionDbContext dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        if (options.Value.RecreateDatabaseEachTime)
        {
            await dbContext.Database.EnsureDeletedAsync(cancellationToken);
        }

        // Ensures that database is created applying the latest state
        // Application of migration later on may fail
        // If you want to use migrations, you should create database by calling  dbContext.Database.MigrateAsync(cancellationToken) instead
        await dbContext.Database.EnsureCreatedAsync(cancellationToken);
    }
}

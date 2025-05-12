using MediaSolution.DAL.Factories;
using MediaSolution.DAL.Mappers;
using MediaSolution.DAL.Migrator;
using MediaSolution.DAL.Options;
using MediaSolution.DAL.Seeds;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace MediaSolution.DAL;

public static class DALInstaller
{
    public static IServiceCollection AddDALServices(this IServiceCollection services)
    {
        services.AddSingleton<IDbContextFactory<MediaSolutionDbContext>>(serviceProvider =>
        {
            var dalOptions = serviceProvider.GetRequiredService<IOptions<DALOptions>>();
            return new DbContextSQLiteFactory(dalOptions.Value.DatabaseFilePath);
        });
        services.AddSingleton<IDbMigrator, DbMigrator>();
        services.AddSingleton<IDbSeeder, DbSeeder>();

        services.AddSingleton<MediaEntityMapper>();
        services.AddSingleton<PlaylistEntityMapper>();
        services.AddSingleton<PlaylistMediaEntityMapper>();

        return services;
    }
}
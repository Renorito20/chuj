using MediaSolution.DAL.Factories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MediaSolution.DAL;


/// <summary>
///     EF Core CLI migration generation uses this DbContext to create model and migration
/// </summary>
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<MediaSolutionDbContext>
{
    private readonly DbContextSQLiteFactory _dbContextSqLiteFactory = new("mediasolution.db");

    public MediaSolutionDbContext CreateDbContext(string[] args) => _dbContextSqLiteFactory.CreateDbContext();
}
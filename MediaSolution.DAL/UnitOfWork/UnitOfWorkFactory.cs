using Microsoft.EntityFrameworkCore;

namespace MediaSolution.DAL.UnitOfWork;

public class UnitOfWorkFactory(IDbContextFactory<MediaSolutionDbContext> dbContextFactory) : IUnitOfWorkFactory
{
    public IUnitOfWork Create() => new UnitOfWork(dbContextFactory.CreateDbContext());
}
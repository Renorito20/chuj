using MediaSolution.BL.Models;
using MediaSolution.DAL.Entities;
using System.Linq.Expressions;

namespace MediaSolution.BL.Facades;

public interface IFacade<TEntity, TListModel, TDetailModel>
    where TEntity : class, IEntity
    where TListModel : IModel
    where TDetailModel : class, IModel
{
    Task DeleteAsync(Guid id);
    Task<TDetailModel?> GetAsync(Guid id);
    Task<IEnumerable<TListModel>> GetAsync();
    Task<TDetailModel> SaveAsync(TDetailModel model);

    // Filtering, sorting, and searching methods
    Task<IEnumerable<TListModel>> GetFilteredAsync(Expression<Func<TEntity, bool>> filterPredicate);
    Task<IEnumerable<TListModel>> GetSortedAsync<TKey>(Expression<Func<TEntity, TKey>> sortKeySelector, bool ascending = true);
    Task<IEnumerable<TListModel>> SearchAsync(string searchTerm);
}
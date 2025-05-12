using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using MediaSolution.BL.Mappers.Interfaces;
using MediaSolution.BL.Models;
using MediaSolution.DAL.Entities;
using MediaSolution.DAL.Mappers;
using MediaSolution.DAL.Repositories;
using MediaSolution.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace MediaSolution.BL.Facades;

// Base facade that implements common CRUD operations with generic entity and model types
// Serves as an abstraction layer between business logic and data access layers
public abstract class
    FacadeBase<TEntity, TListModel, TDetailModel, TEntityMapper>(
        IUnitOfWorkFactory unitOfWorkFactory,
        IModelMapper<TEntity, TListModel, TDetailModel> modelMapper)
    : IFacade<TEntity, TListModel, TDetailModel>
    where TEntity : class, IEntity
    where TListModel : IModel
    where TDetailModel : class, IModel
    where TEntityMapper : IEntityMapper<TEntity>, new()
{
    // Model mapper for converting between entity and model types
    protected readonly IModelMapper<TEntity, TListModel, TDetailModel> ModelMapper = modelMapper;
    
    // Factory for creating unit of work instances to manage database transactions
    protected readonly IUnitOfWorkFactory UnitOfWorkFactory = unitOfWorkFactory;

    // Collection of navigation paths to include when fetching entity details
    // Can be overridden by derived classes to specify entity relationships to eager load
    protected virtual ICollection<string> IncludesNavigationPathDetail => new List<string>();
    
    // Expression that defines which property to use for search operations
    // Null by default, should be overridden by derived classes that support search
    protected virtual Expression<Func<TEntity, string>> SearchProperty => null;

    // Deletes an entity by its ID
    // Throws an exception if the deletion operation fails (e.g., due to constraint violations)
    public async Task DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("The ID cannot be empty.", nameof(id));
        }

        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        try
        {
            Console.WriteLine($"Attempting to delete entity with ID: {id}");

            // Attempt to delete the entity
            await uow.GetRepository<TEntity, TEntityMapper>().DeleteAsync(id).ConfigureAwait(false);

            // Commit the transaction
            try
            {
                await uow.CommitAsync().ConfigureAwait(false);
            }
            catch (Exception commitException)
            {
                Console.WriteLine($"Commit failed: {commitException.Message}");
                throw new InvalidOperationException("Failed to commit the transaction.", commitException);
            }

            Console.WriteLine($"Entity with ID: {id} successfully deleted.");
        }
        catch (Exception e) when (e is DbUpdateException || e is ArgumentException || e is InvalidOperationException)
        {
            Console.WriteLine($"Error deleting entity with ID: {id}. Exception: {e.Message}");
            throw new InvalidOperationException("Entity deletion failed.", e);
        }
    }


    // Retrieves a detailed model by entity ID
    // Includes related entities based on IncludesNavigationPathDetail
    // Returns null if the entity doesn't exist
    public virtual async Task<TDetailModel?> GetAsync(Guid id)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();

        IQueryable<TEntity> query = uow.GetRepository<TEntity, TEntityMapper>().Get();

        // Include all specified navigation properties for eager loading
        foreach (string includePath in IncludesNavigationPathDetail)
        {
            query = query.Include(includePath);
        }

        TEntity? entity = await query.SingleOrDefaultAsync(e => e.Id == id).ConfigureAwait(false);

        return entity is null
            ? null
            : ModelMapper.MapToDetailModel(entity);
    }

    // Retrieves all entities as list models
    // No filtering or eager loading is applied by default
    public virtual async Task<IEnumerable<TListModel>> GetAsync()
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        List<TEntity> entities = await uow
            .GetRepository<TEntity, TEntityMapper>()
            .Get()
            .ToListAsync().ConfigureAwait(false);

        return ModelMapper.MapToListModel(entities);
    }

    // Saves a model - creates a new entity or updates an existing one
    // Performs validation to ensure collections aren't being set directly
    // Returns the saved entity mapped back to a detail model
    public virtual async Task<TDetailModel> SaveAsync(TDetailModel model)
    {
        TDetailModel result;

        // Check that no collections are being set (not supported by this infrastructure)
        GuardCollectionsAreNotSet(model);

        TEntity entity = ModelMapper.MapToEntity(model);

        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IRepository<TEntity> repository = uow.GetRepository<TEntity, TEntityMapper>();

        if (await repository.ExistsAsync(entity).ConfigureAwait(false))
        {
            // Update existing entity
            TEntity updatedEntity = await repository.UpdateAsync(entity).ConfigureAwait(false);
            result = ModelMapper.MapToDetailModel(updatedEntity);
        }
        else
        {
            // Insert new entity with a new GUID
            entity.Id = Guid.NewGuid();
            TEntity insertedEntity = repository.Insert(entity);
            result = ModelMapper.MapToDetailModel(insertedEntity);
        }

        await uow.CommitAsync().ConfigureAwait(false);

        return result;
    }
    
    // Retrieves entities that match the provided filter expression
    // Falls back to GetAsync if no filter is provided
    public virtual async Task<IEnumerable<TListModel>> GetFilteredAsync(Expression<Func<TEntity, bool>> filterPredicate)
    {
        if (filterPredicate == null)
        {
            return await GetAsync();
        }

        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        List<TEntity> entities = await uow
            .GetRepository<TEntity, TEntityMapper>()
            .Get()
            .Where(filterPredicate)
            .ToListAsync()
            .ConfigureAwait(false);

        return ModelMapper.MapToListModel(entities);
    }

    // Retrieves entities sorted by the specified key selector
    // Supports both ascending and descending order
    // Falls back to GetAsync if no sort key is provided
    public virtual async Task<IEnumerable<TListModel>> GetSortedAsync<TKey>(
        Expression<Func<TEntity, TKey>> sortKeySelector, 
        bool ascending = true)
    {
        if (sortKeySelector == null)
        {
            return await GetAsync();
        }

        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IQueryable<TEntity> query = uow.GetRepository<TEntity, TEntityMapper>().Get();

        // Apply appropriate ordering based on the ascending flag
        query = ascending 
            ? query.OrderBy(sortKeySelector) 
            : query.OrderByDescending(sortKeySelector);

        List<TEntity> entities = await query
            .ToListAsync()
            .ConfigureAwait(false);

        return ModelMapper.MapToListModel(entities);
    }

    // Searches for entities containing the provided search term in the property specified by SearchProperty
    // Returns all entities if searchTerm is empty or SearchProperty is not defined
    public virtual async Task<IEnumerable<TListModel>> SearchAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm) || SearchProperty == null)
        {
            return await GetAsync();
        }

        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IQueryable<TEntity> query = uow.GetRepository<TEntity, TEntityMapper>().Get();

        // Extract parameter from the SearchProperty expression
        var parameterExpr = SearchProperty.Parameters[0];
        var propertyExpr = SearchProperty.Body;
        
        // Build a dynamic expression for the Contains method call
        var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
        var constantExpr = Expression.Constant(searchTerm, typeof(string));
        var containsExpr = Expression.Call(propertyExpr, containsMethod, constantExpr);
        
        // Create a final lambda expression that can be used in Where method
        var searchExpr = Expression.Lambda<Func<TEntity, bool>>(containsExpr, parameterExpr);

        List<TEntity> entities = await query
            .Where(searchExpr)
            .ToListAsync()
            .ConfigureAwait(false);

        return ModelMapper.MapToListModel(entities);
    }
    
    // Validates that no collections are being set on the model
    // This is a limitation of the current infrastructure implementation
    private static void GuardCollectionsAreNotSet(TDetailModel model)
    {
        // Find all properties that implement ICollection
        IEnumerable<PropertyInfo> collectionProperties = model
            .GetType()
            .GetProperties()
            .Where(i => typeof(ICollection).IsAssignableFrom(i.PropertyType));

        // Check if any collection property has items
        foreach (PropertyInfo collectionProperty in collectionProperties)
        {
            if (collectionProperty.GetValue(model) is ICollection { Count: > 0 })
            {
                throw new InvalidOperationException(
                    "Current BL and DAL infrastructure disallows insert or update of models with adjacent collections.");
            }
        }
    }
}
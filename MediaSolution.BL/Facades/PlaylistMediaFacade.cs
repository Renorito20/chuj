using System.Linq.Expressions;
using MediaSolution.BL.Mappers;
using MediaSolution.BL.Models;
using MediaSolution.DAL.Entities;
using MediaSolution.DAL.Mappers;
using MediaSolution.DAL.Repositories;
using MediaSolution.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace MediaSolution.BL.Facades;

public class PlaylistMediaFacade(
    IUnitOfWorkFactory unitOfWorkFactory,
    PlaylistMediaModelMapper modelMapper)
    : FacadeBase<PlaylistMediaEntity, PlaylistMediaList, PlaylistMediaDetail,
        PlaylistMediaEntityMapper>(unitOfWorkFactory, modelMapper), IPlaylistMediaFacade
{
    public async Task SaveAsync(PlaylistMediaList model, Guid playlistId)
    {
        PlaylistMediaEntity entity = modelMapper.MapToEntity(model, playlistId);

        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IRepository<PlaylistMediaEntity> repository =
            uow.GetRepository<PlaylistMediaEntity, PlaylistMediaEntityMapper>();

        if (await repository.ExistsAsync(entity))
        {
            await repository.UpdateAsync(entity);
            await uow.CommitAsync();
        }
    }

    public async Task SaveAsync(PlaylistMediaDetail model, Guid playlistId)
    {
        PlaylistMediaEntity entity = modelMapper.MapToEntity(model, playlistId);

        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IRepository<PlaylistMediaEntity> repository =
            uow.GetRepository<PlaylistMediaEntity, PlaylistMediaEntityMapper>();

        repository.Insert(entity);
        await uow.CommitAsync();
    }
    
    public async Task DeleteAsync(Guid id)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        IRepository<PlaylistMediaEntity> repository =
            uow.GetRepository<PlaylistMediaEntity, PlaylistMediaEntityMapper>();

        await repository.DeleteAsync(id);
        await uow.CommitAsync();
    }
    
    // Additional methods for filtering, sorting and searching
    
    // Get all media for a given playlist
    public async Task<IEnumerable<PlaylistMediaList>> GetMediaForPlaylistAsync(Guid playlistId)
    {
        return await GetFilteredAsync(e => e.PlaylistId == playlistId);
    }
    
    // Get media in playlist sorted by order
    public async Task<IEnumerable<PlaylistMediaList>> GetSortedMediaForPlaylistAsync(Guid playlistId, bool ascending = true)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        
        var query = uow.GetRepository<PlaylistMediaEntity, PlaylistMediaEntityMapper>().Get()
            .Where(e => e.PlaylistId == playlistId);
            
        query = ascending 
            ? query.OrderBy(e => e.Order)
            : query.OrderByDescending(e => e.Order);
            
        var entities = await query.ToListAsync().ConfigureAwait(false);
        
        return ModelMapper.MapToListModel(entities);
    }
    
    // change order of media in playlist
    public async Task ChangeMediaOrderAsync(Guid playlistMediaId, int newOrder)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        var repository = uow.GetRepository<PlaylistMediaEntity, PlaylistMediaEntityMapper>();
        
        var entity = await repository.Get()
            .SingleOrDefaultAsync(e => e.Id == playlistMediaId)
            .ConfigureAwait(false);
            
        if (entity != null)
        {
            entity.Order = newOrder;
            await repository.UpdateAsync(entity);
            await uow.CommitAsync();
        }
    }
}
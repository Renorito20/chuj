using System.Linq.Expressions;
using MediaSolution.BL.Mappers;
using MediaSolution.BL.Models;
using MediaSolution.DAL.Entities;
using MediaSolution.DAL.Mappers;
using MediaSolution.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace MediaSolution.BL.Facades;

public class PlaylistFacade(
    IUnitOfWorkFactory unitOfWorkFactory,
    PlaylistModelMapper modelMapper)
    : FacadeBase<PlaylistEntity, PlaylistListModel, PlaylistDetailModel, PlaylistEntityMapper>(
        unitOfWorkFactory, modelMapper), IPlaylistFacade
{
    protected override ICollection<string> IncludesNavigationPathDetail =>
        new[] {$"{nameof(PlaylistEntity.Media)}.{nameof(PlaylistMediaEntity.Media)}"};
        
    // Defining the search property in the PlaylistEntity entity
    protected override Expression<Func<PlaylistEntity, string>> SearchProperty => entity => entity.Name;
    
    // Specific methods for PlaylistFacade
    
    // Get favorite playlists
    public async Task<IEnumerable<PlaylistListModel>> GetFavoritePlaylistsAsync()
    {
        return await GetFilteredAsync(e => e.Favorite);
    }
    
    // Sort playlists by creation date
    public Task<IEnumerable<PlaylistListModel>> GetPlaylistsOrderedByCreationDateAsync(bool ascending = true)
    {
        return GetSortedAsync(e => e.CreatedAt, ascending);
    }
    
    // Sort playlists by update date
    public Task<IEnumerable<PlaylistListModel>> GetPlaylistsOrderedByUpdateDateAsync(bool ascending = true)
    {
        return GetSortedAsync(e => e.UpdatedAt, ascending);
    }
    
    // Filter playlists by description
    public async Task<IEnumerable<PlaylistListModel>> SearchByDescriptionAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return await GetAsync();
        }
        
        return await GetFilteredAsync(e => e.Description != null && e.Description.Contains(searchTerm));
    }
    
    // Search for playlists containing a specific medium
    public async Task<IEnumerable<PlaylistListModel>> GetPlaylistsContainingMediaAsync(Guid mediaId)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();
        
        var query = uow.GetRepository<PlaylistEntity, PlaylistEntityMapper>().Get()
            .Include($"{nameof(PlaylistEntity.Media)}")
            .Where(p => p.Media.Any(pm => pm.MediaId == mediaId));
            
        var entities = await query.ToListAsync().ConfigureAwait(false);
        
        return ModelMapper.MapToListModel(entities);
    }
    public async Task<IEnumerable<MediaListModel>> GetMediaByPlaylistIdAsync(Guid playlistId)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();

        var playlist = await uow.GetRepository<PlaylistEntity, PlaylistEntityMapper>().Get()
          .Include($"{nameof(PlaylistEntity.Media)}.{nameof(PlaylistMediaEntity.Media)}")
          .FirstOrDefaultAsync(p => p.Id == playlistId)
          .ConfigureAwait(false);

        if (playlist == null)
        {
            return Enumerable.Empty<MediaListModel>();
        }

        return playlist.Media.Select(pm => new MediaListModel()
        {
            Id = pm.MediaId,
            Name = pm.Media.Name,
            Authors = pm.Media.Authors,
            Genres = pm.Media.Genres,
            Duration = pm.Media.Duration
        }).ToList();
    }

    public async Task RemoveMediaFromPlaylistAsync(Guid playlistId, Guid mediaId)
    {
        await using IUnitOfWork uow = UnitOfWorkFactory.Create();

        // Properly specify both type arguments for GetRepository
        var repository = uow.GetRepository<PlaylistMediaEntity, PlaylistMediaEntityMapper>();

        var playlistMedia = await repository.Get()
            .FirstOrDefaultAsync(pm => pm.PlaylistId == playlistId && pm.MediaId == mediaId);

        if (playlistMedia != null)
        {
            await repository.DeleteAsync(playlistMedia.Id); // Use DeleteAsync with the entity's ID
            await uow.CommitAsync().ConfigureAwait(false);
        }
    }
}
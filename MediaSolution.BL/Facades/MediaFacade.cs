using System.Linq.Expressions;
using MediaSolution.BL.Mappers;
using MediaSolution.BL.Models;
using MediaSolution.DAL.Entities;
using MediaSolution.DAL.Mappers;
using MediaSolution.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace MediaSolution.BL.Facades;

public class MediaFacade(
    IUnitOfWorkFactory unitOfWorkFactory,
    MediaModelMapper modelMapper)
    : FacadeBase<MediaEntity, MediaListModel, MediaDetailModel, MediaEntityMapper>(
        unitOfWorkFactory, modelMapper), IMediaFacade
{
    // Defining a property for searching in the MediaEntity entity
    protected override Expression<Func<MediaEntity, string>> SearchProperty => entity => entity.Name;
    
    // Other methods specific to MediaFacade

    // Search by author
    public async Task<IEnumerable<MediaListModel>> SearchByAuthorAsync(string author)
    {
        if (string.IsNullOrWhiteSpace(author))
        {
            return await GetAsync();
        }
        
        return await GetFilteredAsync(e => e.Authors != null && e.Authors.Contains(author));
    }
    
    // Search by genre
    public async Task<IEnumerable<MediaListModel>> SearchByGenreAsync(string genre)
    {
        if (string.IsNullOrWhiteSpace(genre))
        {
            return await GetAsync();
        }
        
        return await GetFilteredAsync(e => e.Genres != null && e.Genres.Contains(genre));
    }
    
    // Media sorting by length
    public Task<IEnumerable<MediaListModel>> GetMediaOrderedByDurationAsync(bool ascending = true)
    {
        return GetSortedAsync(e => e.Duration, ascending);
    }
    
    // Sort media by creation date
    public Task<IEnumerable<MediaListModel>> GetMediaOrderedByCreationDateAsync(bool ascending = true)
    {
        return GetSortedAsync(e => e.CreatedAt, ascending);
    }
    
    // Filter media by file size
    public Task<IEnumerable<MediaListModel>> GetMediaFilteredBySizeAsync(long minSize, long maxSize)
    {
        return GetFilteredAsync(e => e.SizeInBytes >= minSize && e.SizeInBytes <= maxSize);
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
}
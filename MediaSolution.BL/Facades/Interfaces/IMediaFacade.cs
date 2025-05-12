using MediaSolution.BL.Models;
using MediaSolution.DAL.Entities;

namespace MediaSolution.BL.Facades;

public interface IMediaFacade : IFacade<MediaEntity, MediaListModel, MediaDetailModel> 
{
    // MediaFacade specific methods
    Task<IEnumerable<MediaListModel>> SearchByAuthorAsync(string author);
    Task<IEnumerable<MediaListModel>> SearchByGenreAsync(string genre);
    Task<IEnumerable<MediaListModel>> GetMediaOrderedByDurationAsync(bool ascending = true);
    Task<IEnumerable<MediaListModel>> GetMediaOrderedByCreationDateAsync(bool ascending = true);
    Task<IEnumerable<MediaListModel>> GetMediaFilteredBySizeAsync(long minSize, long maxSize);
    Task<IEnumerable<MediaListModel>> GetMediaByPlaylistIdAsync(Guid playlistId);

}
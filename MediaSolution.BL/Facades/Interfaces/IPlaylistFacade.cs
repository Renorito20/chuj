using MediaSolution.BL.Models;
using MediaSolution.DAL.Entities;

namespace MediaSolution.BL.Facades;

public interface IPlaylistFacade : IFacade<PlaylistEntity, PlaylistListModel, PlaylistDetailModel> 
{
    // Specific methods for PlaylistFacade
    Task<IEnumerable<PlaylistListModel>> GetFavoritePlaylistsAsync();
    Task<IEnumerable<PlaylistListModel>> GetPlaylistsOrderedByCreationDateAsync(bool ascending = true);
    Task<IEnumerable<PlaylistListModel>> GetPlaylistsOrderedByUpdateDateAsync(bool ascending = true);
    Task<IEnumerable<PlaylistListModel>> SearchByDescriptionAsync(string searchTerm);
    Task<IEnumerable<PlaylistListModel>> GetPlaylistsContainingMediaAsync(Guid mediaId);
    Task<IEnumerable<MediaListModel>> GetMediaByPlaylistIdAsync(Guid playlistId);
    Task RemoveMediaFromPlaylistAsync(Guid playlistId, Guid mediaId);

}
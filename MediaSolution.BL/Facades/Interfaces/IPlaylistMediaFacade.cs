using MediaSolution.BL.Models;

namespace MediaSolution.BL.Facades;

public interface IPlaylistMediaFacade
{
    Task SaveAsync(PlaylistMediaDetail model, Guid playlistId);
    Task SaveAsync(PlaylistMediaList model, Guid playlistId);
    Task DeleteAsync(Guid id);
    
    // Additional methods for filtering, sorting and searching
    Task<IEnumerable<PlaylistMediaList>> GetMediaForPlaylistAsync(Guid playlistId);
    Task<IEnumerable<PlaylistMediaList>> GetSortedMediaForPlaylistAsync(Guid playlistId, bool ascending = true);
    Task ChangeMediaOrderAsync(Guid playlistMediaId, int newOrder);
}
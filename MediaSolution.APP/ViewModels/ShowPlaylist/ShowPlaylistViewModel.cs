// Import necessary namespaces
using CommunityToolkit.Mvvm.ComponentModel;  // For MVVM base functionality
using CommunityToolkit.Mvvm.Input;          // For command attributes
using CommunityToolkit.Mvvm.Messaging;      // For messaging
using MediaSolution.APP.Services;          // Application services
using MediaSolution.APP.Services.Interfaces; // Service interfaces
using MediaSolution.BL.Facades;            // Business layer facades
using MediaSolution.BL.Models;             // Business layer models
using System.Collections.ObjectModel;       // For observable collections

namespace MediaSolution.APP.ViewModels;

// ViewModel for displaying and managing a single playlist
public partial class ShowPlaylistViewModel(
    IPlaylistFacade playlistFacade,        // Facade for playlist operations
    INavigationService navigationService,  // Service for navigation
    IMessengerService messengerService)    // Service for messaging
    : ViewModelBase(messengerService)      // Inherits from base ViewModel
{
    // Private backing fields for properties
    private string _playlistName;          // Stores the playlist name
    private string _playlistDescription;   // Stores the playlist description
    private PlaylistListModel _currentPlaylist; // Stores the current playlist model

    // Observable collection of media items in the playlist
    // Automatically updates UI when modified
    public ObservableCollection<MediaListModel> MediaItems { get; } = new();

    // Playlist name property with change notification
    public string PlaylistName
    {
        get => _playlistName;
        set => SetProperty(ref _playlistName, value); // Updates with property change notification
    }

    // Playlist description property with change notification
    public string PlaylistDescription
    {
        get => _playlistDescription;
        set => SetProperty(ref _playlistDescription, value); // Updates with property change notification
    }

    // Current playlist property with extended setter logic
    public PlaylistListModel CurrentPlaylist
    {
        get => _currentPlaylist;
        set
        {
            // Only update if the value actually changed
            if (SetProperty(ref _currentPlaylist, value))
            {
                // Update dependent properties when playlist changes
                PlaylistName = value?.Name ?? "Unknown Playlist";
                PlaylistDescription = value?.Description ?? "No description available";

                // Load media items asynchronously when playlist changes
                LoadMediaItemsAsync().ConfigureAwait(false);
            }
        }
    }

    // Loads media items for the current playlist
    private async Task LoadMediaItemsAsync()
    {
        if (CurrentPlaylist != null)
        {
            // Clear existing items
            MediaItems.Clear();

            // Fetch media items from facade
            var media = await playlistFacade.GetMediaByPlaylistIdAsync(CurrentPlaylist.Id);

            if (media != null)
            {
                // Add fetched items to observable collection
                foreach (var mediaItem in media)
                {
                    MediaItems.Add(mediaItem);
                }
            }
        }
    }

    // Initializes the ViewModel with a playlist
    public void Initialize(PlaylistListModel playlist)
    {
        // Set the current playlist which triggers loading of media items
        CurrentPlaylist = playlist;
    }



    public async Task DeleteMediaAsync(MediaListModel mediaItem)
    {
        if (mediaItem == null || CurrentPlaylist == null)
            return;

        try
        {
            // Remove the relationship instead of deleting the media
            await playlistFacade.RemoveMediaFromPlaylistAsync(CurrentPlaylist.Id, mediaItem.Id);

            // Reload the playlist data to ensure consistency
            var updatedMedia = await playlistFacade.GetMediaByPlaylistIdAsync(CurrentPlaylist.Id);

            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                MediaItems.Clear();
                foreach (var item in updatedMedia)
                {
                    MediaItems.Add(item);
                }
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error removing media from playlist: {ex.Message}");
        }
    }

}
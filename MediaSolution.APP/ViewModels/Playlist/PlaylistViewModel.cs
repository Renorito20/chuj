// Import necessary namespaces
using CommunityToolkit.Mvvm.ComponentModel;  // For MVVM base functionality
using CommunityToolkit.Mvvm.Input;          // For command attributes
using CommunityToolkit.Mvvm.Messaging;      // For messaging
using MediaSolution.APP.Services;          // Application services
using MediaSolution.APP.Services.Interfaces; // Service interfaces
using MediaSolution.BL.Facades;            // Business layer facades
using MediaSolution.BL.Models;             // Business layer models
using System.Collections.ObjectModel;       // For observable collections
using MediaSolution.APP.Views;

namespace MediaSolution.APP.ViewModels;

// Main ViewModel for managing playlists with filtering capabilities
public partial class PlaylistViewModel : ViewModelBase
{
    // Service dependencies injected via constructor
    private readonly IPlaylistFacade _playlistFacade;  // Facade for playlist operations
    private readonly INavigationService _navigationService;  // Service for view navigation
    private readonly IMediaFacade _mediaFacade;  // Facade for media operations

    // Auto-generated ObservableProperty for all playlists
    [ObservableProperty]
    private IEnumerable<PlaylistListModel> _playlists = [];

    // Auto-generated ObservableProperty for filtered playlists
    [ObservableProperty]
    private IEnumerable<PlaylistListModel> _filteredPlaylists = [];

    // Auto-generated ObservableProperty for search text
    [ObservableProperty]
    private string _searchText = string.Empty;

    // Auto-generated ObservableProperty for available authors
    [ObservableProperty]
    private ObservableCollection<string> _authors = new();

    // Auto-generated ObservableProperty for available genres
    [ObservableProperty]
    private ObservableCollection<string> _genres = new();

    // Auto-generated ObservableProperty for selected author filter
    [ObservableProperty]
    private string _selectedAuthor;

    // Auto-generated ObservableProperty for selected genre filter
    [ObservableProperty]
    private string _selectedGenre;

    // Auto-generated ObservableProperty for minimum length filter
    [ObservableProperty]
    private int? _lengthMin;

    // Auto-generated ObservableProperty for maximum length filter
    [ObservableProperty]
    private int? _lengthMax;

    // Dictionary to store playlist lengths (in minutes)
    private Dictionary<Guid, int> _playlistLengths = new();

    // Dictionary to store authors for each playlist
    private Dictionary<Guid, HashSet<string>> _playlistAuthors = new();

    // Dictionary to store genres for each playlist
    private Dictionary<Guid, HashSet<string>> _playlistGenres = new();

    // Constructor with dependency injection
    public PlaylistViewModel(
        IPlaylistFacade playlistFacade,
        INavigationService navigationService,
        IMediaFacade mediaFacade,
        IMessengerService messengerService) : base(messengerService)
    {
        // Initialize service dependencies
        _playlistFacade = playlistFacade;
        _navigationService = navigationService;
        _mediaFacade = mediaFacade;
    }

    // Override of base LoadDataAsync method to load initial data
    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        // Load all playlists from facade
        Playlists = await _playlistFacade.GetAsync();

        // Populate metadata about playlists (lengths, authors, genres)
        await PopulatePlaylistMetadataAsync();

        // Initialize filter dropdowns with unique values
        Authors = new ObservableCollection<string>(GetUniqueAuthors());
        Genres = new ObservableCollection<string>(GetUniqueGenres());

        // Initially show all playlists (no filters applied)
        FilteredPlaylists = Playlists;
    }

    // Gets all unique authors from all playlists
    private IEnumerable<string> GetUniqueAuthors()
    {
        List<string> uniqueAuthors = new List<string>();

        // Collect all unique authors from all playlists
        foreach (var authorSet in _playlistAuthors.Values)
        {
            foreach (var author in authorSet)
            {
                if (!uniqueAuthors.Contains(author))
                {
                    uniqueAuthors.Add(author);
                }
            }
        }

        // Sort alphabetically for better user experience
        uniqueAuthors.Sort();
        return uniqueAuthors;
    }

    // Gets all unique genres from all playlists
    private IEnumerable<string> GetUniqueGenres()
    {
        List<string> uniqueGenres = new List<string>();

        // Collect all unique genres from all playlists
        foreach (var genreSet in _playlistGenres.Values)
        {
            foreach (var genre in genreSet)
            {
                if (!uniqueGenres.Contains(genre))
                {
                    uniqueGenres.Add(genre);
                }
            }
        }

        // Sort alphabetically for better user experience
        uniqueGenres.Sort();
        return uniqueGenres;
    }

    // Populates metadata dictionaries for filtering
    private async Task PopulatePlaylistMetadataAsync()
    {
        // Clear existing metadata
        _playlistLengths.Clear();
        _playlistAuthors.Clear();
        _playlistGenres.Clear();

        // Process each playlist
        foreach (var playlist in Playlists)
        {
            // Get all media items for this playlist
            var mediaItems = await _mediaFacade.GetMediaByPlaylistIdAsync(playlist.Id);

            int totalLengthInMinutes = 0;
            var playlistAuthors = new HashSet<string>();
            var playlistGenres = new HashSet<string>();

            // Process each media item in the playlist
            foreach (var media in mediaItems)
            {
                // Calculate total length in minutes
                totalLengthInMinutes += (int)media.Duration.TotalSeconds / 60;

                // Add authors if present
                if (!string.IsNullOrWhiteSpace(media.Authors))
                {
                    playlistAuthors.Add(media.Authors);
                }

                // Add genres if present
                if (!string.IsNullOrWhiteSpace(media.Genres))
                {
                    playlistGenres.Add(media.Genres);
                }
            }

            // Store calculated metadata
            _playlistLengths[playlist.Id] = totalLengthInMinutes;
            _playlistAuthors[playlist.Id] = playlistAuthors;
            _playlistGenres[playlist.Id] = playlistGenres;
        }
    }

    // Command to clear all filters
    [RelayCommand]
    public void ClearFilters()
    {
        // Reset all filter properties
        SearchText = string.Empty;
        SelectedAuthor = null;
        SelectedGenre = null;
        LengthMin = 0;
        LengthMax = 0;

        // Show all playlists again
        FilteredPlaylists = Playlists;
    }

    // Command to apply current filters
    [RelayCommand]
    public async void ApplyFilters()
    {
        await LoadDataAsync();
        List<PlaylistListModel> result = new List<PlaylistListModel>();

        // Check each playlist against current filters
        foreach (var playlist in Playlists)
        {
            bool includePlaylist = true;

            // Search text filter
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                if (!playlist.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) &&
                    !playlist.Description.Contains(SearchText, StringComparison.OrdinalIgnoreCase))
                {
                    includePlaylist = false;
                }
            }

            // Author filter
            if (includePlaylist && !string.IsNullOrWhiteSpace(SelectedAuthor))
            {
                if (!_playlistAuthors.ContainsKey(playlist.Id) ||
                    !_playlistAuthors[playlist.Id].Contains(SelectedAuthor))
                {
                    includePlaylist = false;
                }
            }

            // Genre filter
            if (includePlaylist && !string.IsNullOrWhiteSpace(SelectedGenre))
            {
                if (!_playlistGenres.ContainsKey(playlist.Id) ||
                    !_playlistGenres[playlist.Id].Contains(SelectedGenre))
                {
                    includePlaylist = false;
                }
            }

            // Duration range filters
            if (includePlaylist && LengthMin > 0 && LengthMax > 0 && LengthMin <= LengthMax)
            {
                if (!_playlistLengths.ContainsKey(playlist.Id) ||
                    _playlistLengths[playlist.Id] < LengthMin ||
                    _playlistLengths[playlist.Id] > LengthMax)
                {
                    includePlaylist = false;
                }
            }
            else if (includePlaylist && LengthMin > 0)
            {
                if (!_playlistLengths.ContainsKey(playlist.Id) ||
                    _playlistLengths[playlist.Id] < LengthMin)
                {
                    includePlaylist = false;
                }
            }
            else if (includePlaylist && LengthMax > 0)
            {
                if (!_playlistLengths.ContainsKey(playlist.Id) ||
                    _playlistLengths[playlist.Id] > LengthMax)
                {
                    includePlaylist = false;
                }
            }

            // Add to results if passed all filters
            if (includePlaylist)
            {
                result.Add(playlist);
            }
        }

        // Update filtered playlists collection
        FilteredPlaylists = result;
    }

    [RelayCommand]
    private async Task ViewMediaAsync()
    {
        await _navigationService.GoToAsync(nameof(ViewMediaView));
    }


    // Helper method to trigger search
    private void Search()
    {
        ApplyFilters();
    }

    // Automatically called when SearchText changes
    partial void OnSearchTextChanged(string value)
    {
        Search();
    }

    public async Task OnDeletePlaylistClicked(Guid playlistId)
    {
        try
        {
            await _playlistFacade.DeleteAsync(playlistId);
            await LoadDataAsync(); // Refresh the data after deletion
        }
        catch (Exception ex)
        {
            // Consider adding error handling or propagating the exception
            throw;
        }
    }
}
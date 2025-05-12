// Import necessary namespaces
using MediaSolution.APP.ViewModels;
using MediaSolution.APP.Views.Playlist;
using MediaSolution.APP.Views.ShowPlaylist;
using MediaSolution.BL.Models;
using Microsoft.Extensions.Logging;
using System;

namespace MediaSolution.APP.Views.Playlist;

// Main view class for displaying and managing playlists
public partial class PlaylistView : ContentPageBase
{
    // Service provider for dependency injection
    private readonly IServiceProvider _serviceProvider;
    // Logger for tracking events and debugging
    private readonly ILogger<PlaylistView> _logger;

    // Constructor with dependency injection
    public PlaylistView(PlaylistViewModel viewModel,
                       IServiceProvider serviceProvider,
                       ILogger<PlaylistView> logger)
        : base(viewModel)
    {
        // Initialize dependencies
        _serviceProvider = serviceProvider;
        _logger = logger;

        // Set up the XAML components
        InitializeComponent();
    }

    // Event handler for clearing all filters
    private void OnClearFiltersClicked(object sender, EventArgs e)
    {
        if (BindingContext is PlaylistViewModel viewModel)
        {
            // Execute the clear filters command
            viewModel.ClearFiltersCommand.Execute(null);
        }
    }

    // Event handler for applying selected filters
    private void OnApplyFiltersClicked(object sender, EventArgs e)
    {
        if (BindingContext is PlaylistViewModel viewModel)
        {
            // Apply fallback values if null or less than zero
            int lengthMin = viewModel.LengthMin.HasValue && viewModel.LengthMin.Value > 0 ? viewModel.LengthMin.Value : 0;
            int lengthMax = viewModel.LengthMax.HasValue && viewModel.LengthMax.Value > 0 ? viewModel.LengthMax.Value : 9999;

            // Log the filter values being applied
            string selectedAuthor = viewModel.SelectedAuthor ?? "None";
            string selectedGenre = viewModel.SelectedGenre ?? "None";

            _logger.LogInformation($"Applying filters - Author: {selectedAuthor}, Genre: {selectedGenre}, Duration: {lengthMin} - {lengthMax} minutes");

            // Optionally update ViewModel with fallback values
            viewModel.LengthMin = lengthMin;
            viewModel.LengthMax = lengthMax;

            // Execute the apply filters command
            viewModel.ApplyFiltersCommand.Execute(null);
        }
    }

    // Event handler for text changes in search field
    private void OnTextChanged(object sender, EventArgs e)
    {
        // Log that the text has changed
        _logger.LogInformation("OnTextChanged fired!");
    }

    // Event handler for search button click
    private void OnSearchClicked(object sender, EventArgs e)
    {
        if (BindingContext is PlaylistViewModel viewModel)
        {
            // Execute the apply filters command (same as search)
            viewModel.ApplyFiltersCommand.Execute(null);
        }
    }

    // Event handler for adding new media
    private async void OnClickAddMedia(object sender, EventArgs e)
    {
        // Get the AddMediaViewModel from service provider
        var addMediaViewModel = _serviceProvider.GetService<AddMediaViewModel>();

        // Create new AddMediaView and navigate to it
        var addMediaView = new AddMediaView(addMediaViewModel);
        await Navigation.PushAsync(addMediaView);
    }

    // Event handler for creating new playlist
    private async void OnClickCreatePlaylist(object sender, EventArgs e)
    {
        // Get the CreatePlaylistViewModel from service provider
        var createPlaylistViewModel = _serviceProvider.GetService<CreatePlaylistViewModel>();

        // Create new CreatePlaylistView and navigate to it
        var createPlaylistView = new CreatePlaylistView(createPlaylistViewModel);
        await Navigation.PushAsync(createPlaylistView);
    }

    private async void OnClickViewMedia(object sender, EventArgs e)
    {
        var viewMediaViewModel = _serviceProvider.GetService<ViewMediaViewModel>();

        var viewMediaPage = new ViewMediaView(viewMediaViewModel, _serviceProvider);
        await Navigation.PushAsync(viewMediaPage);
    }

    // Event handler when a playlist is tapped/selected
    private async void OnPlaylistTapped(object sender, EventArgs e)
    {
        // Check if sender is a Frame control
        if (sender is not Frame frame)
        {
            return;
        }

        // Check if Frame has a PlaylistListModel as its BindingContext
        if (frame.BindingContext is not PlaylistListModel playlist)
        {
            return;
        }

        // Get the ShowPlaylistViewModel from service provider
        var showPlaylistViewModel = _serviceProvider.GetService<ShowPlaylistViewModel>();

        // Create new ShowPlaylistView with the selected playlist
        var showPlaylistView = new ShowPlaylistView(showPlaylistViewModel, _serviceProvider);

        // Initialize the view with the selected playlist
        showPlaylistView.Initialize(playlist);

        // Navigate to the playlist details view
        await Navigation.PushAsync(showPlaylistView);
    }

    public async void OnDeletePlaylistClicked(object sender, EventArgs e)
    {
        // Check if sender is a Button control
        if (sender is not Button button)
        {
            return;
        }

        // Check if Button has a PlaylistListModel as its BindingContext
        if (button.BindingContext is not PlaylistListModel playlist)
        {
            return;
        }

        // Confirm deletion with the user
        bool confirmDelete = await DisplayAlert(
            "Delete Playlist",
            $"Are you sure you want to delete the playlist '{playlist.Name}'?",
            "Yes",
            "No"
        );

        if (!confirmDelete)
        {
            return;
        }

        try
        {
            // Get the PlaylistViewModel from the BindingContext
            if (BindingContext is PlaylistViewModel viewModel)
            {
                // Call the delete method in the ViewModel
                await viewModel.OnDeletePlaylistClicked(playlist.Id);

                // Update the Playlists collection to reflect the deletion
                viewModel.Playlists = viewModel.Playlists.Where(p => p.Id != playlist.Id).ToList();

                // Trigger UI update by reassigning the collection
                viewModel.FilteredPlaylists = viewModel.Playlists;

                // Log the successful deletion
                _logger.LogInformation($"Playlist '{playlist.Name}' deleted successfully.");
            }
        }
        catch (Exception ex)
        {
            // Log the error
            _logger.LogError(ex, $"Error occurred while deleting playlist '{playlist.Name}'.");

            // Notify the user of the error
            await DisplayAlert("Error", "An error occurred while deleting the playlist. Please try again.", "OK");
        }
    }
}
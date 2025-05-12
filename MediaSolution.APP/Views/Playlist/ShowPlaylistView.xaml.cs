// Import necessary namespaces
using MediaSolution.APP.ViewModels;
using MediaSolution.APP.Views.ShowMedia;
using MediaSolution.BL.Models;
using Microsoft.Extensions.Logging;
using System.Diagnostics.Tracing;

namespace MediaSolution.APP.Views.ShowPlaylist;

// Partial class definition for ShowPlaylistView, inheriting from ContentPageBase
public partial class ShowPlaylistView : ContentPageBase
{
    // Private fields to store view model and service provider
    private ShowPlaylistViewModel _viewModel;
    private readonly IServiceProvider _serviceProvider;

    // Constructor that takes view model and service provider as dependencies
    public ShowPlaylistView(
        ShowPlaylistViewModel viewModel,
        IServiceProvider serviceProvider)
        : base(viewModel)
    {
        // Initialize class fields with injected dependencies
        _viewModel = viewModel;
        _serviceProvider = serviceProvider;

        // Initialize the XAML components
        InitializeComponent();
    }

    // Public method to initialize the view with a playlist
    public void Initialize(PlaylistListModel playlist)
    {
        // Pass the playlist to the view model for initialization
        _viewModel.Initialize(playlist);
    }

    // Event handler for when the view button is clicked
    private async void OnViewButtonClicked(object sender, EventArgs e)
    {
        // Get the button that triggered the event
        var button = (Button)sender;

        // Get the media item associated with the button
        var media = (MediaListModel)button.CommandParameter;

        // Resolve the ShowMediaViewModel from the service provider
        var showMediaViewModel = _serviceProvider.GetService<ShowMediaViewModel>();

        // Initialize the view model with the selected media
        showMediaViewModel.Initialize(media);

        // Create a new ShowMediaView with the initialized view model
        var showMediaView = new ShowMediaView(showMediaViewModel);

        // Navigate to the new view
        await Navigation.PushAsync(showMediaView);
    }

    private async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        // Get the button that triggered the event
        var button = (Button)sender;

        // Get the media item associated with the button
        var media = (MediaListModel)button.CommandParameter;

        // Confirm deletion with the user
        bool confirm = await DisplayAlert(
            "Delete Media",
            $"Are you sure you want to delete '{media.Name}'?",
            "Yes",
            "No");

        if (!confirm)
            return;

        try
        {
            // Remove the media item from the ViewModel's collection
            _viewModel.MediaItems.Remove(media);

            // Call the ViewModel's method to handle deletion logic
            await _viewModel.DeleteMediaAsync(media);

            // Notify the user of successful deletion
            await DisplayAlert("Success", $"'{media.Name}' has been deleted.", "OK");
        }
        catch (Exception ex)
        {
            // Log the error and notify the user
            Console.WriteLine($"Error deleting media: {ex.Message}");
            await DisplayAlert("Error", "An error occurred while deleting the media. Please try again.", "OK");
        }
    }
}
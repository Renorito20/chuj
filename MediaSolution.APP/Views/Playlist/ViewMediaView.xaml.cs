using MediaSolution.APP.ViewModels;
using MediaSolution.APP.Views.ShowMedia;
using MediaSolution.BL.Models;


namespace MediaSolution.APP.Views;

public partial class ViewMediaView : ContentPageBase
{
    private readonly ViewMediaViewModel _viewModel;
    private readonly IServiceProvider _serviceProvider;

    public ViewMediaView(ViewMediaViewModel viewModel, IServiceProvider serviceProvider) : base(viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        _serviceProvider = serviceProvider;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _viewModel.LoadAsync();
    }
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

}
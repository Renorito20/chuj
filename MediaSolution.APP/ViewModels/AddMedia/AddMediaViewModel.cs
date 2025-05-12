using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MediaSolution.APP.Services;
using MediaSolution.APP.Services.Interfaces;
using MediaSolution.BL.Facades;
using MediaSolution.BL.Models;

namespace MediaSolution.APP.ViewModels;

// ViewModel for adding a new media item
public partial class AddMediaViewModel : ViewModelBase
{
    private readonly IMediaFacade _mediaFacade;
    private readonly INavigationService _navigationService;

    // Constructor with dependency injection
    public AddMediaViewModel(
        IMediaFacade mediaFacade,
        INavigationService navigationService,
        IMessengerService messengerService)
        : base(messengerService)
    {
        _mediaFacade = mediaFacade;
        _navigationService = navigationService;
    }

    // Properties for media input fields (binded from UI)

    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private string authors = string.Empty;

    [ObservableProperty]
    private string genres = string.Empty;

    [ObservableProperty]
    private string duration = string.Empty; // will be parsed to TimeSpan

    [ObservableProperty]
    private string sizeInBytes = string.Empty; // will be parsed to long

    [ObservableProperty]
    private string path = string.Empty;

    // Save command called from button
    [RelayCommand]
    public async Task SaveAsync()
    {
        if (!double.TryParse(Duration, out var parsedDuration))
        {
            await ShowError("Invalid duration");
            return;
        }

        if (!long.TryParse(SizeInBytes, out var parsedSize))
        {
            await ShowError("Invalid size in bytes");
            return;
        }

        var newMedia = new MediaDetailModel
        {
            Id = Guid.Empty,
            Name = Name,
            Authors = Authors,
            Genres = Genres,
            Duration = TimeSpan.FromMinutes(parsedDuration),
            SizeInBytes = parsedSize,
            Path = Path,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _mediaFacade.SaveAsync(newMedia);
        await _navigationService.GoBackAsync();
    }


    // Cancel command called from button
    //todo

    // Simple error popup
    private async Task ShowError(string message)
    {
        await Shell.Current.DisplayAlert("Error", message, "OK");
    }
}

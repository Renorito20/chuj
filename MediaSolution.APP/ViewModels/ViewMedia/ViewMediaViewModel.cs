using CommunityToolkit.Mvvm.ComponentModel;
using MediaSolution.BL.Facades;
using MediaSolution.BL.Models;
using System.Collections.ObjectModel;
using MediaSolution.APP.Services.Interfaces;

namespace MediaSolution.APP.ViewModels;

public partial class ViewMediaViewModel : ViewModelBase
{
    private readonly IMediaFacade _mediaFacade;

    [ObservableProperty]
    private ObservableCollection<MediaListModel> media = [];

    public ViewMediaViewModel(IMediaFacade mediaFacade,
                                IMessengerService messengerService) : base(messengerService)
    {
        _mediaFacade = mediaFacade;
    }

    public async Task LoadAsync()
    {
        var mediaItems = await _mediaFacade.GetAsync();
        Media = new ObservableCollection<MediaListModel>(mediaItems);
    }
}

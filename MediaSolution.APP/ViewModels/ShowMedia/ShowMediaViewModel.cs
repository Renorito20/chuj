using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MediaSolution.APP.Services;
using MediaSolution.APP.Services.Interfaces;
using MediaSolution.BL.Facades;
using MediaSolution.BL.Models;

namespace MediaSolution.APP.ViewModels;

public partial class ShowMediaViewModel(
    IPlaylistFacade playlistFacade,
    INavigationService navigationService,
    IMessengerService messengerService)
    : ViewModelBase(messengerService)
{
    public MediaListModel CurrentMedia { get; set; }


    public void Initialize(MediaListModel media)
    {
        CurrentMedia = media;
    }
}
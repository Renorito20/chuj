using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MediaSolution.APP.Services.Interfaces;
using MediaSolution.BL.Facades;
using MediaSolution.BL.Models;
using Microsoft.Maui.Controls;
using CommunityToolkit.Mvvm.Messaging;


namespace MediaSolution.APP.ViewModels
{
    public partial class CreatePlaylistViewModel : ViewModelBase
    {
        private readonly IPlaylistFacade _playlistFacade;
        private readonly INavigationService _navigationService;

        public CreatePlaylistViewModel(
            IPlaylistFacade playlistFacade,
            INavigationService navigationService,
            IMessengerService messengerService)
            : base(messengerService)
        {
            _playlistFacade = playlistFacade;
            _navigationService = navigationService;
        }

        [ObservableProperty]
        private string name = string.Empty;

        [ObservableProperty]
        private string description = string.Empty;

        [ObservableProperty]
        private string coverImage = string.Empty;

        /// <summary>
        /// The list of songs in this new playlist
        /// </summary>
        public ObservableCollection<SongItemViewModel> Songs { get; } = new();

        [RelayCommand]
        private async Task AddSongAsync()
        {
            // TODO: implement your song-picker here—
            // for now, we'll just add a dummy item so you can see layout:
            var idx = Songs.Count + 1;
            Songs.Add(new SongItemViewModel(idx, $"Pesnièka {idx}"));
        }

        [RelayCommand]
        public async Task SaveAsync()
        {

            if (string.IsNullOrWhiteSpace(Name))
            {
                await Shell.Current.DisplayAlert("Error", "Playlist name is required", "OK");
                return;
            }

            var newPlaylist = new PlaylistDetailModel
            {
                Id = Guid.Empty,
                Name = Name,
                Description = Description,
                CoverImage = CoverImage,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // pass your songs along however your facade needs them...
            await _playlistFacade.SaveAsync(newPlaylist);
            await _navigationService.GoBackAsync();
        }

        [RelayCommand]
        private async Task GoBackAsync()
            => await _navigationService.GoBackAsync();
    }

    // A little item-VM for each song row:
    public partial class SongItemViewModel : ObservableObject
    {
        public SongItemViewModel(int index, string title)
        {
            Index = index;
        }

        public int Index { get; }

        [ObservableProperty]
        private bool isSelected;

        [ObservableProperty]
        private string title;
    }
}

using MediaSolution.APP.ViewModels;

namespace MediaSolution.APP.Views.Playlist
{
    public partial class CreatePlaylistView : ContentPageBase
    {
        public CreatePlaylistView(CreatePlaylistViewModel viewModel)
            : base(viewModel)
        {
            InitializeComponent();
        }
    }
}

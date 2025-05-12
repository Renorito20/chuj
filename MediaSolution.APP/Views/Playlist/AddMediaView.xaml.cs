using MediaSolution.APP.ViewModels;
using Microsoft.Extensions.Logging;

namespace MediaSolution.APP.Views.Playlist;

public partial class AddMediaView : ContentPageBase
{
    public AddMediaView(AddMediaViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();

    }

}
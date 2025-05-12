using MediaSolution.APP.ViewModels;
using Microsoft.Extensions.Logging;

namespace MediaSolution.APP.Views.ShowMedia;

public partial class ShowMediaView : ContentPageBase
{
    public ShowMediaView(ShowMediaViewModel viewModel)
        : base(viewModel)
    {
        InitializeComponent();
    }


}
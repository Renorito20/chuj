using MediaSolution.APP.ViewModels;
using Microsoft.Maui.Controls;

namespace MediaSolution.APP.Views;

public abstract partial class ContentPageBase
{
    protected ViewModelBase ViewModel { get; }

    public ContentPageBase(ViewModelBase viewModel)
    {
        InitializeComponent();

        BindingContext = ViewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        
        await ViewModel.OnAppearingAsync();
    }
}
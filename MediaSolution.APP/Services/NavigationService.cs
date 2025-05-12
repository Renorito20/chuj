using MediaSolution.APP.Models;
using MediaSolution.APP.Services.Interfaces;
using MediaSolution.APP.Views.Playlist;
using MediaSolution.APP.Views;

namespace MediaSolution.APP.Services;


public class NavigationService : INavigationService
{
    // todo add routes 

    public IEnumerable<RouteModel> Routes { get; } = new List<RouteModel>
    {
        new(nameof(AddMediaView), typeof(AddMediaView)),
        new(nameof(ViewMediaView), typeof(ViewMediaView)), 
    };

    public async Task GoToAsync(string route)
        => await Shell.Current.GoToAsync(route);

    public async Task GoToAsync(string route, IDictionary<string, object?> parameters)
        => await Shell.Current.GoToAsync(route, parameters);

    public bool SendBackButtonPressed()
        => Shell.Current.SendBackButtonPressed();

    public async Task GoBackAsync()
        => await Shell.Current.GoToAsync("..");

}
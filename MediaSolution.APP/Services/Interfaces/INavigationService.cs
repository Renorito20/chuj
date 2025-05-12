using MediaSolution.APP.Models;
namespace MediaSolution.APP.Services.Interfaces;

public interface INavigationService
{
    IEnumerable<RouteModel> Routes { get; }

    Task GoToAsync(string route);
    Task GoToAsync(string route, IDictionary<string, object?> parameters);

    Task GoBackAsync();
    bool SendBackButtonPressed();
}
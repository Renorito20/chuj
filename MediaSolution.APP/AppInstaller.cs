using CommunityToolkit.Mvvm.Messaging;
using MediaSolution.APP.Extensions;
using MediaSolution.APP.Services;
using MediaSolution.APP.Services.Interfaces;

namespace MediaSolution.APP;

public static class AppInstaller
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddSingleton<AppShell>();

        services.AddSingleton<IMessenger>(_ => WeakReferenceMessenger.Default);

        services.AddSingleton<IMessengerService, MessengerService>();
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<IAlertService, AlertService>();

        services.AddViews();        
        services.AddViewModels();
        
        return services;
    }
}
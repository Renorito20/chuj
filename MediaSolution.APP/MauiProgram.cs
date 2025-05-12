using System.Reflection;
using CommunityToolkit.Maui;
using MediaSolution.APP.Services.Interfaces;
using MediaSolution.BL;
using MediaSolution.DAL;
using MediaSolution.DAL.Migrator;
using MediaSolution.DAL.Options;
using MediaSolution.DAL.Seeds;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MediaSolution.APP.Views.ShowMedia;
using MediaSolution.APP.Views;
using MediaSolution.APP.ViewModels;
using MediaSolution.APP.Views.Playlist;

namespace MediaSolution.APP;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()

	{
        Console.WriteLine($"Database path: {Path.Combine(FileSystem.AppDataDirectory, "mediasolution.db")}");

        var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		
		ConfigureAppSettings(builder);
		
		builder.Services
			.AddDALServices()
			.AddBLServices()
			.AddAppServices();
        builder.Services.AddTransient<ShowMediaViewModel>();
        builder.Services.AddTransient<ShowMediaView>();

        builder.Services.AddTransient<AddMediaViewModel>();
        builder.Services.AddTransient<AddMediaView>();

		builder.Services.AddTransient<CreatePlaylistViewModel>();
        builder.Services.AddTransient<CreatePlaylistView>();

        builder.Services.AddTransient<ViewMediaViewModel>();
        builder.Services.AddTransient<ViewMediaView>(provider =>
										new ViewMediaView(
											provider.GetRequiredService<ViewMediaViewModel>(), provider));

#if DEBUG
        builder.Logging.AddDebug();
#endif

        var app = builder.Build();
        
        AssertDALOptionsConfiguration(app);
		MigrateDb(app.Services.GetRequiredService<IDbMigrator>());
		SeedDb(app.Services.GetRequiredService<IDbSeeder>());
		RegisterRouting(app.Services.GetRequiredService<INavigationService>());
		
		return app;
	}

	private static void ConfigureAppSettings(MauiAppBuilder builder)
	{
		var configurationBuilder = new ConfigurationBuilder();

		var assembly = Assembly.GetExecutingAssembly();
		
		const string appSettingsFilePath = "MediaSolution.APP.appsettings.json";
		
		using var appSettingsStream = assembly.GetManifestResourceStream(appSettingsFilePath);

		if (appSettingsStream is not null)
		{
			configurationBuilder.AddJsonStream(appSettingsStream);
		}

		var configuration = configurationBuilder.Build();
		builder.Configuration.AddConfiguration(configuration);
		
		builder.Services.Configure<DALOptions>(builder.Configuration.GetSection("MediaSolution:DAL"));
	}
	
	private static void RegisterRouting(INavigationService navigationService)
	{
		foreach (var route in navigationService.Routes)
		{
			Routing.RegisterRoute(route.Route, route.ViewType);
		}
	}

	private static void MigrateDb(IDbMigrator migrator) => migrator.Migrate();
	private static void SeedDb(IDbSeeder dbSeeder) => dbSeeder.SeedDatabase();
	
	private static void AssertDALOptionsConfiguration(MauiApp app)
	{
		var dalOptions = app.Services.GetRequiredService<IOptions<DALOptions>>();

		if (dalOptions?.Value is null)
		{
			throw new InvalidOperationException("No persistence provider configured");
		}

		if (string.IsNullOrEmpty(dalOptions?.Value.DatabaseName))
		{
			throw new InvalidOperationException($"{nameof(DALOptions.DatabaseName)} is not set");
		}
	}
}

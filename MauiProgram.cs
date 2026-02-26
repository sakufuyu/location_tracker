using Microsoft.Maui.Controls.Maps;
using LocationTracker.Services;

namespace LocationTracker;

/// <summary>
/// A class that provides initialization for a MAUI application.
/// </summary>
public static class MauiProgram
{
    /// <summary>
    /// Configure the MAUI app and register the required services.
    /// </summary>
    /// <returns>A configured <see cref="MauiApp"/> instance.</returns>
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            // Register application class
            .UseMauiApp<App>()
            // Activate Map service
            .UseMauiMaps()
            // Register fonts to be used throughout the app
            .ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
        });

        // Add single connection to the database
        builder.Services.AddSingleton<LocationDatabase>();
        // Add single connection to the Location Service
        builder.Services.AddSingleton<LocationService>();
        // Register the main page
        builder.Services.AddTransient<MainPage>();

        return builder.Build();
    }
}
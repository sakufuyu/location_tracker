using Microsoft.Maui.Controls.Maps;

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

        return builder.Build();
    }
}
using Foundation;

namespace LocationTracker;

/// <summary>
/// Entry point for an app of iOS platform.
/// Bridging the UIKit lifecycle with .NET MAUI applications.
/// </summary>
[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
    /// <summary>
    /// Generate MAUI application
    /// Call the common entry point <see cref="MauiProgram.CreateMauiApp"/>.
    /// <returns>The configured <see cref="MauiApp"/> instance.</returns>
    protected override MauiApp CreateMauiApp()
        => MauiProgram.CreateMauiApp();
}
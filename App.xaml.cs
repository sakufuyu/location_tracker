using System.Security.Principal;

namespace LocationTracker;

/// <summary>
/// Root class of the app.
/// Responsible for configuring the MAUI app lifecycle and initial window.
/// </summary>
public partial class App : Application
{
    /// <summary>
    /// Initialize new instance of App class
    /// Load resources and configurations defined in XAML.
    /// </summary>
    public App()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Creates the initial <see cref="Window"/> that is created when the app launches.
    /// 
    /// MAUI allows you to override this method to control the page and shell configuration displayed at launch.
    /// </summary>
    /// <param name="activationState">
    /// The launch context passed by the platform.
    /// May be null for normal launches.
    /// </param>
    /// <returns>
    /// A <see cref="Window"/> with a <see cref="AppShell"/> as its root.
    /// </returns>
    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }
}
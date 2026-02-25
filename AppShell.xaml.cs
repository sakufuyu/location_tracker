namespace LocationTracker;

/// <summary>
/// Defines the application's navigation structure using MAUI Shell.
/// This class connects AppShell.xaml (UI definition) to the runtime.
/// </summary>
public partial class AppShell : Shell
{
    /// <summary>
    /// Initializes the Shell and loads the XAML-defined navigation layout.
    /// </summary>
    public AppShell()
    {
        InitializeComponent(); // Loads AppShell.xaml
    }
}
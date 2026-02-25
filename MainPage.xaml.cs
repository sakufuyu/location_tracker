using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using Microsoft.Maui.Devices.Sensors;

namespace LocationTracker;

/// <summary>
/// For a main page of app
/// UI definition is written in MainPage.xaml
/// </summary>
public partial class MainPage : ContentPage
{
    /// <summary>
    /// Initialize new instance of MainPage
    /// Load UI component defined in XAML
    /// Combine with the auto-generated partial class.
    /// </summary>
    public MainPage()
    {
        // Load UI defined in XAML and initialize the control reference
        InitializeComponent();

        // Seattle as initial display
        var seattle = new Location(47.6062, -122.3321);

        MyMap.MoveToRegion(
            MapSpan.FromCenterAndRadius(seattle, Distance.FromKilometers(5))
        );
    }
}
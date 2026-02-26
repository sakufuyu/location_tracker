using LocationTracker.Models;
using LocationTracker.Services;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Maps;

namespace LocationTracker;

/// <summary>
/// Main UI page coordinating:
/// - Location tracking service
/// - Database persistence
/// - Real-time UI updates
/// 
/// Architectural role:
/// Acts as a presentation layer that subscribes to service events.
/// Business logic remains inside LocationService.
/// </summary>
public partial class MainPage : ContentPage
{
    private readonly LocationService _locationService;
    private readonly LocationDatabase _locationDatabase;

    // In-memory counter for UI display
    private int _pointCount = 0;

    // Location cluster counter for heatmap
    private readonly Dictionary<string, int> _heatCounter = new();

    /// <summary>
    /// Constructor with dependency injection.
    /// Services are provided by the MAUI DI container.
    /// </summary>
    public MainPage(LocationService locationService, LocationDatabase locationDatabase)
    {
        // Load UI defined in XAML and initialize the control reference
        InitializeComponent();

        _locationService = locationService;
        _locationDatabase = locationDatabase;

        // Subscribe to live GPS updates
        _locationService.LocationChanged += OnLocationChanged;

        // Reload saved path from SQLite
        LoadSavedPoints();
    }

    /// <summary>
    /// Handles new location samples emitted by LocationService.
    /// 
    /// Responsibilities:
    /// - Update UI labels
    /// - Move map viewport
    /// - Maintain local point counter
    /// 
    /// UI updates must run on the main thread.
    /// </summary>
    private void OnLocationChanged(object? sender, LocationRecord record)
    {
        _pointCount++;

        MainThread.BeginInvokeOnMainThread(() =>
        {
            StatusLabel.Text = $"Lat: {record.Latitude:F6}  Lon: {record.Longitude:F6}";
            PointCountLabel.Text = $"Points saved: {_pointCount}";

            // Draw current location on Map
            AddHeatPoint(record);

            // Re-center map around latest location
            MyMap.MoveToRegion(
                MapSpan.FromCenterAndRadius(
                    new Location(record.Latitude, record.Longitude),
                    Distance.FromKilometers(1)
                )
            );
            AddHeatPoint(record);
        });
    }

    /// <summary>
    /// Load previously saved locations from SQLite
    /// and redraw them on the map.
    /// Called when the page start.
    /// </summary>
    private async void LoadSavedPoints()
    {
        var records = await _locationDatabase.GetLocationsAsync();

        foreach (var record in records)
        {
            AddHeatPoint(record);
        }

        _pointCount = records.Count;
        PointCountLabel.Text = $"Points saved: {_pointCount}";
    }

    /// <summary>
    /// Adds a visual heat point to the map.
    ///
    /// Workflow:
    /// 1. Convert GPS coordinate into grid cell.
    /// 2. Increase visit counter for that cell.
    /// 3. Determine heat intensity color.
    /// 4. Render a colored circle overlay on the map.
    ///
    /// Design decision:
    /// Instead of drawing complex heatmap overlays,
    /// this lightweight approach uses MapElements.Circle
    /// to simulate heat density visualization.
    /// 
    /// Advantage:
    /// - Cross-platform
    /// - Simple implementation
    /// - Meets assignment heatmap requirement
    /// </summary>
    private void AddHeatPoint(LocationRecord record)
    {
        var key = GetGridKey(record);

        if (!_heatCounter.ContainsKey(key))
        {
            _heatCounter[key] = 0;
        }
        _heatCounter[key]++;

        var count = _heatCounter[key];
        var color = GetHeatColor(count);

        var circle = new Circle
        {
            Center = new Location(record.Latitude, record.Longitude),
            Radius = new Distance(10),
            StrokeColor = Colors.Transparent,
            FillColor = color
        };

        MyMap.MapElements.Add(circle);
    }

    /// <summary>
    /// To change the color of points on the map,
    /// latitude and longitude are rounded to 4 decimal places
    /// </summary>
    /// <param name="record">Location sample obtained from GPS.</param>
    /// <returns>
    /// A string key representing an approximate geographic cell
    /// (~11 meters resolution).
    /// </returns>
    private string GetGridKey(LocationRecord record)
    {
        // Round 0.0001 ≒ Approx 11 meters
        var lat = Math.Round(record.Latitude, 4);
        var lon = Math.Round(record.Longitude, 4);

        return $"{lat}_{lon}";
    }

    /// <summary>
    /// Determines heatmap color based on visit count.
    ///
    /// Heat scale:
    /// 1–9   : Blue   (low activity)
    /// 10–19 : Yellow
    /// 20–29 : Orange
    /// 30–39 : Red
    /// 40+   : Purple (high activity)
    /// </summary>
    /// <param name="count">
    /// Number of recorded visits within the same geographic grid.
    /// </param>
    /// <returns>
    /// A color representing location intensity,
    /// ranging from low activity (blue)
    /// to high activity (purple).
    /// </returns>
    private Color GetHeatColor(int count)
    {
        if (count < 10)
            return Colors.Blue;

        if (count < 20)
            return Colors.Yellow;

        if (count < 30)
            return Colors.Orange;

        if (count < 40)
            return Colors.Red;

        return Colors.Purple;
    }

    /// <summary>
    /// Handles Start/Stop toggle button.
    /// 
    /// Flow:
    /// - If tracking -> stop service
    /// - If not tracking -> Chech permission -> start service
    /// 
    /// Location permission is requested at runtime.
    /// </summary>
    private async void OnToggleClicked(object? sender, EventArgs e)
    {
        if (_locationService.IsTracking)
        {
            _locationService.StopTracking();

            ToggleButton.Text = "Start Tracking";
            ToggleButton.BackgroundColor = Color.FromArgb("#512BD4");

            StatusLabel.Text = "Tracking stopped.";

            return;
        }
        else
        {
             // Runtime permission check
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

            if (status != PermissionStatus.Granted)
            {
                await DisplayAlertAsync(
                    "Permission Required",
                    "Location permission is required to track your position.",
                    "OK"
                );
                return;
            }
        }

        _locationService.StartTracking();

        ToggleButton.Text = "Stop Tracking";
        ToggleButton.BackgroundColor = Color.FromArgb("#D4432B");

        StatusLabel.Text = "Tracking started - waiting for first reading...";
    }
}
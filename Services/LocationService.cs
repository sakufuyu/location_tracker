using System.ComponentModel.DataAnnotations;
using LocationTracker.Models;

namespace LocationTracker.Services;

/// <summary>
/// Periodically polls GPS coordinates and persists them to the local database.
/// 
/// Responsibilities:
/// - Retrieves device location at a configurable interval
/// - Stores raw location samples
/// - Emits events for real-time UI updateds
/// 
/// Design notes:
/// - Uses poling instead of continuous background tracking for simplicity.
/// - Fire-and-foreget loops is controlled via CancellationToken.
/// - Timestamp is recorded in UTC for consistency.
/// 
/// How LocationService Works:
/// [Call StartTracking()]
///         ↓
/// while (until canceled) {
/// Georlocation.GetLocationAsync() // Get GPS
/// → Create LocationRecord
/// → Save to DB
/// → Fire LocationChanged event // Notify UI
/// → Wait 10 seconds
/// }
/// [Call StopTracking()]
///         ↓
/// Stop the loop with a CancellationToken
/// </summary>
public class LocationService
{
    private readonly LocationDatabase _db;
    private CancellationTokenSource? _cts;
    private bool _isTracking;

    /// <summary>
    /// Polling interval for location retrieval.
    /// Default is 10 seconds.
    /// 
    /// Note:
    /// - Shorter intervals increase accuracy but drain battery.
    /// - Longer intervals reduce DB growth and power usage.
    /// </summary>
    public TimeSpan Interval { get; set; } = TimeSpan.FromSeconds(10);

    /// <summary>
    /// Raised whenever a new location sample is successfully captured and saved.
    /// UI layers can subscribe to this for live updates.
    /// </summary>
    public event EventHandler<LocationRecord>? LocationChanged;

    /// <summary>
    /// Indicates whther tracking is currently activate.
    /// </summary>
    public bool IsTracking => _isTracking;

    /// <summary>
    /// Constructor with dependency injection.
    /// LocationDatabase is injected to decouple persistence logic.
    /// </summary>
    public LocationService(LocationDatabase db)
    {
        _db = db;
    }

    /// <summary>
    /// Starts the location polling loop.
    /// Prevents duplicate loops from starting.
    /// </summary>
    public void StartTracking()
    {
        if (_isTracking) return;

        _isTracking = true;
        _cts = new CancellationTokenSource();

        // Fire-and-forget async loop
        _ = PollLoopAsync(_cts.Token);
    }

    /// <summary>
    /// Stops the location polling loop gracefully.
    /// </summary>
    public void StopTracking()
    {
        if (!_isTracking) return;

        _isTracking = false;

        _cts?.Cancel();
        _cts?.Dispose();
        _cts = null;
    }

    /// <summary>
    /// Main pooling loop.
    /// 
    /// Loop behavior:
    /// 1. Request location from MAUI Geolocation API
    /// 2. Persist result
    /// 3. Notify subscribers
    /// 4. Wait for the configured interval
    /// </summary>
    private async Task PollLoopAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            try
            {
                // Request high-accuracy location
                var request = new GeolocationRequest(
                    GeolocationAccuracy.Best,
                    Interval
                );

                var location = await Geolocation.Default.GetLocationAsync(request, ct);

                if (location is not null)
                {
                    var record = new LocationRecord
                    {
                        Latitude = location.Latitude,
                        Longitude = location.Longitude,
                        Timestamp = DateTime.UtcNow,
                        Accuracy = location.Accuracy ?? 0
                    };

                    await _db.SaveLocationAsync(record);

                    // Notify subscribers (UI)
                    LocationChanged?.Invoke(this, record);
                }
            }
            catch (OperationCanceledException)
            {
                // Expected during StopTracking()
                break;
            }
            catch (Exception ex)
            {
                // Non-fatal error are logged and loop continues
                System.Diagnostics.Debug.WriteLine(
                    $"[LocationService] Error: {ex.Message}"
                );
            }

            try
            {
                await Task.Delay(Interval, ct);
            }
            catch (OperationCanceledException)
            {
                break;
            }
        }
    }
}
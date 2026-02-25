using SQLite;

namespace LocationTracker.Models;

/// <summary>
/// Represents a single GPS location reading stored in the local database.
/// Mapped to the SQLite table "LocationRecords".
/// 
/// Design notes:
/// - Store raw coordinate data for later aggregation (e.g., heatmap generation).
/// - Timestamp is expected to be stored in UTC to avoid timezon inconsistencies.
/// - Accuracy is preserved to allow filtering of low-quality GPS samples.
/// </summary>
[Table("LocationRecords")] // Explicit table name for schema stability
public class LocationRecord
{
    /// <summary>
    /// Auto-incremented primary key.
    /// Used for indexing and efficient querying.
    /// </summary>
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    /// <summary>
    /// Latitude in decimal degrees (WGS84).
    /// </summary>
    public double Latitude { get; set; }

    /// <summary>
    /// Longitude in decimal degrees (WGS84).
    /// </summary>
    public double Longitude { get; set; }

    /// <summary>
    /// Timestamp of the GPS reading.
    /// Should be stored in UTC to ensure consistency across regions.
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Horizontal accuracy in meters.
    /// Lower values indicate higher precision.
    /// Useful for filtering unreliable readings.
    /// </summary>
    public double Accuracy { get; set; }
}
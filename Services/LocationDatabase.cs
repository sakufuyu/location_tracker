using SQLite;
using LocationTracker.Models;

namespace LocationTracker.Services;

/// <summary>
/// Provides asynchronous access to the local SQLite database.
/// 
/// Design intent:
/// - Encapsulates all persistence logic for location records.
/// - Lazily initializes the database connection.
/// - Intended to be registered as a singleton in DI.
/// </summary>
public class LocationDatabase
{
    private SQLiteAsyncConnection? _database;

    /// <summary>
    /// Returns the SQLite connection, creating it if necessary.
    /// 
    /// Lazy initialization avoids:
    /// - Opening the database before it is needed.
    /// - Startup performance penalties.
    /// 
    /// Ensure the required table schema exists.
    /// </summary>
    private async Task<SQLiteAsyncConnection> GetConnectionAsync()
    {
        if (_database is not null)
        {
            return _database;
        }

        // AppDataDirectory ensure:
        // - Pre-app sandbox storage
        // - OS-managed lifecycle
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "locations.db3");
        _database = new SQLiteAsyncConnection(dbPath);

        // Idempotent table creation (safe to call multiple times)
        await _database.CreateTableAsync<LocationRecord>();

        return _database;
    }

    /// <summary>
    /// Inserts a single location record into the database.
    /// </summary>
    public async Task<int> SaveLocationAsync(LocationRecord record)
    {
        var db = await GetConnectionAsync();
        return await db.InsertAsync(record);
    }

    /// <summary>
    /// Retrieves all stored location records.
    /// 
    /// NOTE:
    /// - This loads the entire dataset into memory.
    /// - For large dataqsets, pagination or filtering is recommended.
    /// </summary>
    public async Task<List<LocationRecord>> GetLocationsAsync()
    {
        var db = await GetConnectionAsync();
        return await db.Table<LocationRecord>().ToListAsync();
    }

    /// <summary>
    /// Deletes all location records.
    /// Intended for development or reset scenarios.
    /// </summary>
    public async Task<int> ClearAsync()
    {
        var db = await GetConnectionAsync();
        return await db.DeleteAllAsync<LocationRecord>();
    }
}
using ObjCRuntime;
using UIKit;

namespace LocationTracker;

/// <summary>
/// Native entry point for iOS app.
/// Passing control from the .NET runtime to the UIKit application lifecycle. 
/// </summary>
public class Program
{
    /// <summary>
    /// This method is executed first when an iOS app launches.
    /// Initializes the UIApplication and specifies the AppDelegate.
    /// Starts app lifecycle management.
    /// </summary>
    /// <param name="args">Launch arguments (usually unused). </param>
    static void Main(string[] args)
    {
        // Call entry point for UIKit
        // Passing control to MAUI app via AppDelegate
        UIApplication.Main(args, null, typeof(AppDelegate));
    }
}
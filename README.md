# Location Tracker (.NET MAUI)
## Overview
Location Tracker is a mobile application developed using C# and .NET MAUI.
The application periodically records the user's GPS location, stores it in a local SQLite database, and displays tracked locations on a map.

This project demonstrates location tracking, database storage, and map visualization using .NET MAUI.

## Features
- Real-time GPS location tracking
- SQLite local data storage
- Map visualization using .NET MAUI Maps
- Start and stop tracking control

## Technologies
- C#
- .NET MAUI
- .NET MAUI Maps
- SQLite (sqlite-net-pcl)

## How to Use
1. Launch the application.
2. Grant location permission when requested.
3. Tap Start Tracking to begin recording locations.
4. The application saves location data and updates the map.
5. Tap Stop Tracking to stop tracking.

## Project Structure
```
Models/
  LocationRecord.cs     - Location data model

Services/
  LocationService.cs    - GPS tracking logic
  LocationDatabase.cs   - SQLite database access

MainPage.xaml           - UI layout
MainPage.xaml.cs        - UI logic
MauiProgram.cs          - App configuration
Platforms/iOS/          - iOS platform configuration
```

## Build and Run
Just in case, clean it:
```
dotnet restore
dotnet clean
```
Build the project:
```
dotnet build -f net10.0-ios
```
Run the application:
```
dotnet build -f net10.0-ios -t:Run
```

# User interface
You track where you were.

If you stay the same location, the mark color changes in the following order in every `100` seconds:

🔵 ➡️ 🟡 ➡️ 🟠 ➡️ 🔴 ➡️ 🟣

<table align="center">
<tr>
<td align="center">
<img src="https://github.com/sakufuyu/location_tracker/blob/main/Resources/UI/IMG_9299.PNG" width="250"><br>
Tracking Screen
</td>

<td align="center">
<img src="https://github.com/sakufuyu/location_tracker/blob/main/Resources/UI/IMG_9298.PNG" width="250"><br>
Larger tracking Screen
</td>

<td align="center">
<img src="https://github.com/sakufuyu/location_tracker/blob/main/Resources/UI/IMG_9294.PNG" width="250"><br>
Heat Map View
</td>

<td align="center">
<img src="https://github.com/sakufuyu/location_tracker/blob/main/Resources/UI/IMG_9293.PNG" width="250"><br>
Heat Map View
</td>

<td align="center">
<img src="https://github.com/sakufuyu/location_tracker/blob/main/Resources/UI/IMG_9297.PNG" width="250"><br>
Heat Map View
</td>
</tr>
</table>
(yes I'm living in Seattle...)
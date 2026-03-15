# TrueMetrics.Maui

.NET MAUI wrapper for the [TRUE Metrics Android SDK](https://docu.truemetrics.cloud/introduction).

Provides a clean, async, DI-friendly C# API surface for the TRUE Metrics sensor recording SDK. The underlying SDK runs only on Android; all other platforms throw `PlatformNotSupportedException` gracefully.

---

## Installation

Install via NuGet:

```bash
dotnet add package TrueMetrics.Maui
```

---

## Setup

In `MauiProgram.cs`:

```csharp
using TrueMetrics.Maui;

builder.Services.AddTrueMetrics(options =>
{
    options.ApiKey = "YOUR_API_KEY";
});
```

---

## Usage

Inject `ITrueMetricsService` into your page or view model:

```csharp
public class MyViewModel
{
    private readonly ITrueMetricsService _metrics;

    public MyViewModel(ITrueMetricsService metrics)
    {
        _metrics = metrics;
    }

    public async Task OnAppStarted()
    {
        await _metrics.InitializeAsync("YOUR_API_KEY");
        await _metrics.StartSessionAsync();
        await _metrics.TrackEventAsync("AppOpened");
    }
}
```

### Track events with properties

```csharp
await _metrics.TrackEventAsync("DeliveryCompleted", new Dictionary<string, string>
{
    ["orderId"] = "ORD-12345",
    ["durationSeconds"] = "420"
});
```

### Set user identity

```csharp
await _metrics.SetUserIdAsync("driver-42");
```

### Stop a session

```csharp
await _metrics.StopSessionAsync();
```

---

## API Reference

| Method | Description |
|---|---|
| `InitializeAsync(apiKey)` | Initialize the SDK. Call once on app start. |
| `StartSessionAsync()` | Begin a recording session. |
| `StopSessionAsync()` | End the current recording session. |
| `TrackEventAsync(name)` | Log a named event. |
| `TrackEventAsync(name, properties)` | Log a named event with metadata. |
| `SetUserIdAsync(userId)` | Associate a user ID with recordings. |
| `IsRecordingInProgress` | Whether a session is active. |
| `DeviceId` | SDK-assigned device identifier. |

---

## Supported Platforms

| Platform | Status |
|---|---|
| Android | ✅ Fully supported |
| iOS | ❌ SDK not available |
| Windows | ❌ SDK not available |
| MacCatalyst | ❌ SDK not available |

Minimum Android API: **24** (Android 7.0)

---

## Android Permissions

The SDK requires the following permissions (declared automatically via the AAR manifest merge):

- `FOREGROUND_SERVICE` / `FOREGROUND_SERVICE_LOCATION`
- `ACCESS_FINE_LOCATION` / `ACCESS_BACKGROUND_LOCATION`
- `HIGH_SAMPLING_RATE_SENSORS`
- `ACTIVITY_RECOGNITION`
- `INTERNET`

You must request runtime permissions for location and activity recognition before calling `StartSessionAsync()`.

---

## Project Structure

```
/src
  TrueMetrics.Maui/                  # Cross-platform MAUI library (NuGet package)
  TrueMetrics.Maui.Android.Binding/  # Android AAR binding library
/sample
  TrueMetrics.Maui.Sample/           # MAUI sample app
README.md
LICENSE
```

---

## Building from Source

```bash
# Install required workloads
dotnet workload install maui android

# Restore & build
dotnet restore TrueMetrics.Maui.sln
dotnet build TrueMetrics.Maui.sln --configuration Release

# Pack NuGet
dotnet pack src/TrueMetrics.Maui/TrueMetrics.Maui.csproj --configuration Release
```

---

## License

[MIT](LICENSE)

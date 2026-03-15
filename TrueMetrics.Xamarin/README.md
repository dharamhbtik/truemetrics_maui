# TrueMetrics.Xamarin

Xamarin.Forms wrapper for the [TRUE Metrics Android SDK](https://docu.truemetrics.cloud/introduction).

Provides a clean, async, DI-friendly C# API for the TRUE Metrics sensor recording SDK targeting Xamarin.Forms apps. The underlying SDK runs only on Android; iOS and the shared netstandard2.1 target throw `PlatformNotSupportedException` gracefully.

---

## Installation

Install via NuGet:

```bash
dotnet add package TrueMetrics.Xamarin
```

---

## Setup

In your shared `App.cs` (or platform `AppDelegate` / `MainActivity`):

```csharp
using Microsoft.Extensions.DependencyInjection;
using TrueMetrics.Xamarin;

var services = new ServiceCollection();

services.AddTrueMetrics(options =>
{
    options.ApiKey = "YOUR_API_KEY";
});

services.AddSingleton<ITrueMetricsService, TrueMetricsService>();
var provider = services.BuildServiceProvider();
```

---

## Usage

Resolve `ITrueMetricsService` from your DI container or pass it via constructor:

```csharp
var metrics = provider.GetRequiredService<ITrueMetricsService>();

await metrics.InitializeAsync("YOUR_API_KEY");
await metrics.StartSessionAsync();
await metrics.TrackEventAsync("AppOpened");
```

### Track events with properties

```csharp
await metrics.TrackEventAsync("DeliveryCompleted", new Dictionary<string, string>
{
    ["orderId"] = "ORD-12345",
    ["durationSeconds"] = "420"
});
```

### Set user identity

```csharp
await metrics.SetUserIdAsync("driver-42");
```

### Stop a session

```csharp
await metrics.StopSessionAsync();
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

| Platform | TFM | Status |
|---|---|---|
| Android | `monoandroid13.0` | ✅ Fully supported |
| iOS | `xamarin.ios10` | ❌ SDK not available |
| Shared / Forms | `netstandard2.1` | Reference target only |

Minimum Android API: **24** (Android 7.0)

---

## Android Permissions

The SDK requires the following permissions (declared automatically via the AAR manifest merge):

- `FOREGROUND_SERVICE` / `FOREGROUND_SERVICE_LOCATION`
- `ACCESS_FINE_LOCATION` / `ACCESS_BACKGROUND_LOCATION`
- `HIGH_SAMPLING_RATE_SENSORS`
- `ACTIVITY_RECOGNITION`
- `INTERNET`

Request runtime permissions for location and activity recognition before calling `StartSessionAsync()`.

---

## Project Structure

```
/src
  TrueMetrics.Xamarin/                  # Cross-platform wrapper library (NuGet package)
  TrueMetrics.Xamarin.Android.Binding/  # Android AAR binding library
/sample
  TrueMetrics.Xamarin.Sample/           # Xamarin.Forms sample app
README.md
LICENSE
```

---

## Building from Source

```bash
# Install required workloads
dotnet workload install android ios

# Restore & build
dotnet restore TrueMetrics.Xamarin.sln
dotnet build TrueMetrics.Xamarin.sln --configuration Release

# Pack NuGet
dotnet pack src/TrueMetrics.Xamarin/TrueMetrics.Xamarin.csproj --configuration Release
```

---

## License

[MIT](LICENSE)

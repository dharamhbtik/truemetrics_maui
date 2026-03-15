using Android.App;
using TrueMetrics.Sdk.Android;
using TrueMetrics.Sdk.Android.Config;

namespace TrueMetrics.Maui;

internal sealed partial class TrueMetricsService
{
    private Task InitializePlatformAsync(string apiKey)
    {
        var context = Application.Context
            ?? throw new InvalidOperationException("Android Application.Context is null.");

        var config = new SdkConfiguration.Builder(apiKey)
            .DelayAutoStartRecording(_options.DelayAutoStartRecordingMs)
            .Build();

        TrueMetricsSdk.Init(context, config);

        _logger.LogInformation("TrueMetrics: Android SDK initialized (v1.4.6).");
        return Task.CompletedTask;
    }

    private Task StartSessionPlatformAsync()
    {
        GetSdkInstance().StartRecording();
        return Task.CompletedTask;
    }

    private Task StopSessionPlatformAsync()
    {
        GetSdkInstance().StopRecording();
        return Task.CompletedTask;
    }

    private Task TrackEventPlatformAsync(string name, Dictionary<string, string>? properties)
    {
        var sdk = GetSdkInstance();
        var javaMap = ToJavaMap(properties ?? new Dictionary<string, string> { ["event"] = name });

        if (properties is { Count: > 0 })
        {
            // Store properties under the event name as a metadata tag, then flush it
            sdk.AppendToMetadataTag(name, javaMap);
            sdk.LogMetadataByTag(name);
        }
        else
        {
            sdk.LogMetadata(javaMap);
        }

        return Task.CompletedTask;
    }

    private Task SetUserIdPlatformAsync(string userId)
    {
        GetSdkInstance().LogMetadata(ToJavaMap(new Dictionary<string, string> { ["userId"] = userId }));
        return Task.CompletedTask;
    }

    partial void GetIsRecordingInProgressPlatform(ref bool result)
    {
        try { result = TrueMetricsSdk.Instance?.IsRecordingInProgress ?? false; }
        catch { result = false; }
    }

    partial void GetDeviceIdPlatform(ref string? result)
    {
        try { result = TrueMetricsSdk.Instance?.DeviceId; }
        catch { result = null; }
    }

    // ── helpers ──────────────────────────────────────────────────────────────

    private static TrueMetricsSdk GetSdkInstance() =>
        TrueMetricsSdk.Instance
            ?? throw new InvalidOperationException(
                "TrueMetrics SDK is not initialized. Call InitializeAsync() first.");

    /// <summary>Converts a .NET dictionary to a java.util.Map&lt;String,String&gt;.</summary>
    private static Java.Util.IMap<Java.Lang.String, Java.Lang.String> ToJavaMap(
        Dictionary<string, string> dict)
    {
        var map = new Java.Util.HashMap<Java.Lang.String, Java.Lang.String>();
        foreach (var (k, v) in dict)
            map.Put(new Java.Lang.String(k), new Java.Lang.String(v));
        return map;
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TrueMetrics.Sdk.Android;
using TrueMetrics.Sdk.Android.Config;

namespace TrueMetrics.Xamarin
{
    internal sealed partial class TrueMetricsService
    {
        private Task InitializePlatformAsync(string apiKey)
        {
            // Android.App.Application.Context is the correct way to get
            // the application context in Xamarin.Android without an Activity reference.
            var context = global::Android.App.Application.Context
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

            if (properties != null && properties.Count > 0)
            {
                sdk.AppendToMetadataTag(name, properties);
                sdk.LogMetadataByTag(name);
            }
            else
            {
                sdk.LogMetadata(new Dictionary<string, string> { ["event"] = name });
            }

            return Task.CompletedTask;
        }

        private Task SetUserIdPlatformAsync(string userId)
        {
            GetSdkInstance().LogMetadata(new Dictionary<string, string> { ["userId"] = userId });
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

        private static TrueMetricsSdk GetSdkInstance() =>
            TrueMetricsSdk.Instance
                ?? throw new InvalidOperationException(
                    "TrueMetrics SDK is not initialized. Call InitializeAsync() first.");
    }
}

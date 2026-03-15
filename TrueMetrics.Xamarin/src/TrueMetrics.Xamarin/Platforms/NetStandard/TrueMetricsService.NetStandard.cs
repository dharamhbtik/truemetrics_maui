using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TrueMetrics.Xamarin
{
    // netstandard2.1 is the shared reference target consumed by Xamarin.Forms
    // shared projects. The real implementation is provided by the platform TFM
    // (monoandroid / xamarin.ios) at link time.
    internal sealed partial class TrueMetricsService
    {
        private Task InitializePlatformAsync(string apiKey) =>
            throw new PlatformNotSupportedException("TrueMetrics SDK is only supported on Android.");

        private Task StartSessionPlatformAsync() =>
            throw new PlatformNotSupportedException("TrueMetrics SDK is only supported on Android.");

        private Task StopSessionPlatformAsync() =>
            throw new PlatformNotSupportedException("TrueMetrics SDK is only supported on Android.");

        private Task TrackEventPlatformAsync(string name, Dictionary<string, string>? properties) =>
            throw new PlatformNotSupportedException("TrueMetrics SDK is only supported on Android.");

        private Task SetUserIdPlatformAsync(string userId) =>
            throw new PlatformNotSupportedException("TrueMetrics SDK is only supported on Android.");

        partial void GetIsRecordingInProgressPlatform(ref bool result) => result = false;

        partial void GetDeviceIdPlatform(ref string? result) => result = null;
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace TrueMetrics.Xamarin
{
    /// <summary>
    /// Cross-platform entry point that delegates to the platform-specific partial implementation.
    /// On non-Android platforms all methods throw <see cref="PlatformNotSupportedException"/>.
    /// </summary>
    internal sealed partial class TrueMetricsService : ITrueMetricsService
    {
        private readonly TrueMetricsOptions _options;
        private readonly ILogger<TrueMetricsService> _logger;

        public TrueMetricsService(TrueMetricsOptions options, ILogger<TrueMetricsService> logger)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        public async Task InitializeAsync(string apiKey)
        {
            if (string.IsNullOrWhiteSpace(apiKey)) throw new ArgumentException("apiKey must not be empty.", nameof(apiKey));
            _logger.LogInformation("TrueMetrics: Initializing SDK.");
            await InitializePlatformAsync(apiKey).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task StartSessionAsync()
        {
            _logger.LogInformation("TrueMetrics: Starting session.");
            await StartSessionPlatformAsync().ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task StopSessionAsync()
        {
            _logger.LogInformation("TrueMetrics: Stopping session.");
            await StopSessionPlatformAsync().ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task TrackEventAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("name must not be empty.", nameof(name));
            _logger.LogDebug("TrueMetrics: Tracking event '{EventName}'.", name);
            await TrackEventPlatformAsync(name, null).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task TrackEventAsync(string name, Dictionary<string, string> properties)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("name must not be empty.", nameof(name));
            if (properties == null) throw new ArgumentNullException(nameof(properties));
            _logger.LogDebug("TrueMetrics: Tracking event '{EventName}' with {Count} properties.", name, properties.Count);
            await TrackEventPlatformAsync(name, properties).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task SetUserIdAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId)) throw new ArgumentException("userId must not be empty.", nameof(userId));
            _logger.LogInformation("TrueMetrics: Setting user ID.");
            await SetUserIdPlatformAsync(userId).ConfigureAwait(false);
        }

        // Platform partial methods — implemented per TFM
        partial void GetIsRecordingInProgressPlatform(ref bool result);
        partial void GetDeviceIdPlatform(ref string? result);

        /// <inheritdoc/>
        public bool IsRecordingInProgress
        {
            get
            {
                bool result = false;
                GetIsRecordingInProgressPlatform(ref result);
                return result;
            }
        }

        /// <inheritdoc/>
        public string? DeviceId
        {
            get
            {
                string? result = null;
                GetDeviceIdPlatform(ref result);
                return result;
            }
        }
    }
}

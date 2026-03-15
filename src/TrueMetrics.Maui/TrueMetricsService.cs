using Microsoft.Extensions.Logging;

namespace TrueMetrics.Maui;

/// <summary>
/// Cross-platform entry point that delegates to the platform-specific implementation.
/// On non-Android platforms all methods throw <see cref="PlatformNotSupportedException"/>.
/// </summary>
internal sealed partial class TrueMetricsService : ITrueMetricsService
{
    private readonly TrueMetricsOptions _options;
    private readonly ILogger<TrueMetricsService> _logger;

    public TrueMetricsService(TrueMetricsOptions options, ILogger<TrueMetricsService> logger)
    {
        _options = options;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task InitializeAsync(string apiKey)
    {
        _logger.LogInformation("TrueMetrics: Initializing SDK.");
        await InitializePlatformAsync(apiKey);
    }

    /// <inheritdoc/>
    public async Task StartSessionAsync()
    {
        _logger.LogInformation("TrueMetrics: Starting session.");
        await StartSessionPlatformAsync();
    }

    /// <inheritdoc/>
    public async Task StopSessionAsync()
    {
        _logger.LogInformation("TrueMetrics: Stopping session.");
        await StopSessionPlatformAsync();
    }

    /// <inheritdoc/>
    public async Task TrackEventAsync(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        _logger.LogDebug("TrueMetrics: Tracking event '{EventName}'.", name);
        await TrackEventPlatformAsync(name, null);
    }

    /// <inheritdoc/>
    public async Task TrackEventAsync(string name, Dictionary<string, string> properties)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(properties);
        _logger.LogDebug("TrueMetrics: Tracking event '{EventName}' with {Count} properties.", name, properties.Count);
        await TrackEventPlatformAsync(name, properties);
    }

    /// <inheritdoc/>
    public async Task SetUserIdAsync(string userId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userId);
        _logger.LogInformation("TrueMetrics: Setting user ID.");
        await SetUserIdPlatformAsync(userId);
    }

    // Partial methods implemented per platform
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

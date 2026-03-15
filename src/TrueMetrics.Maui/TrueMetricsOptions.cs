namespace TrueMetrics.Maui;

/// <summary>
/// Configuration options for the TRUE Metrics SDK.
/// </summary>
public sealed class TrueMetricsOptions
{
    /// <summary>
    /// Your TRUE Metrics API key. Required.
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// Optional delay in milliseconds before auto-starting recording after initialization.
    /// Set to 0 to disable auto-start (use <see cref="ITrueMetricsService.StartSessionAsync"/> explicitly).
    /// Defaults to 0 (explicit start required).
    /// </summary>
    public long DelayAutoStartRecordingMs { get; set; } = 0;
}

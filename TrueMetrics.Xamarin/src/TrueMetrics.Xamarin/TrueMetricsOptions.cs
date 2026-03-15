namespace TrueMetrics.Xamarin
{
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
        /// Set to 0 to require an explicit <see cref="ITrueMetricsService.StartSessionAsync"/> call.
        /// Defaults to 0.
        /// </summary>
        public long DelayAutoStartRecordingMs { get; set; } = 0;
    }
}

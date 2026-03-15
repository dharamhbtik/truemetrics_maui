using System.Collections.Generic;
using System.Threading.Tasks;

namespace TrueMetrics.Xamarin
{
    /// <summary>
    /// Cross-platform interface for the TRUE Metrics SDK.
    /// Android is fully supported; other platforms throw <see cref="System.PlatformNotSupportedException"/>.
    /// </summary>
    public interface ITrueMetricsService
    {
        /// <summary>
        /// Initializes the SDK with the provided API key.
        /// Must be called before any other method.
        /// </summary>
        /// <param name="apiKey">Your TRUE Metrics API key.</param>
        Task InitializeAsync(string apiKey);

        /// <summary>
        /// Starts a recording session.
        /// </summary>
        Task StartSessionAsync();

        /// <summary>
        /// Stops the current recording session.
        /// </summary>
        Task StopSessionAsync();

        /// <summary>
        /// Tracks a named event with no additional properties.
        /// </summary>
        /// <param name="name">Event name.</param>
        Task TrackEventAsync(string name);

        /// <summary>
        /// Tracks a named event with additional metadata properties.
        /// </summary>
        /// <param name="name">Event name (used as metadata tag).</param>
        /// <param name="properties">Key-value metadata to attach to the event.</param>
        Task TrackEventAsync(string name, Dictionary<string, string> properties);

        /// <summary>
        /// Associates a user identifier with subsequent recordings.
        /// </summary>
        /// <param name="userId">Unique user identifier.</param>
        Task SetUserIdAsync(string userId);

        /// <summary>
        /// Returns whether a recording session is currently in progress.
        /// </summary>
        bool IsRecordingInProgress { get; }

        /// <summary>
        /// Returns the device identifier assigned by the SDK.
        /// </summary>
        string? DeviceId { get; }
    }
}

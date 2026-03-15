using System;
using Microsoft.Extensions.DependencyInjection;

namespace TrueMetrics.Xamarin
{
    /// <summary>
    /// Extension methods for registering TRUE Metrics services with the DI container.
    /// </summary>
    public static class TrueMetricsServiceCollectionExtensions
    {
        /// <summary>
        /// Registers the TRUE Metrics SDK service and configuration.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configure">Action to configure <see cref="TrueMetricsOptions"/>.</param>
        /// <returns>The service collection for chaining.</returns>
        /// <example>
        /// <code>
        /// services.AddTrueMetrics(options =>
        /// {
        ///     options.ApiKey = "YOUR_API_KEY";
        /// });
        /// </code>
        /// </example>
        public static IServiceCollection AddTrueMetrics(
            this IServiceCollection services,
            Action<TrueMetricsOptions> configure)
        {
            if (configure == null) throw new ArgumentNullException(nameof(configure));

            var options = new TrueMetricsOptions();
            configure(options);

            if (string.IsNullOrWhiteSpace(options.ApiKey))
                throw new ArgumentException("TrueMetricsOptions.ApiKey must not be empty.", nameof(configure));

            services.AddSingleton(options);
            services.AddSingleton<ITrueMetricsService, TrueMetricsService>();

            return services;
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TrueMetrics.Xamarin;
using Xamarin.Forms;

namespace TrueMetrics.Xamarin.Sample
{
    public class App : Application
    {
        public static ServiceProvider Services { get; private set; } = null!;

        public App()
        {
            var services = new ServiceCollection();

            services.AddLogging(b => b.AddDebug().SetMinimumLevel(LogLevel.Debug));

            services.AddTrueMetrics(options =>
            {
                options.ApiKey = "YOUR_API_KEY_HERE";
            });

            services.AddTransient<MainPage>();

            Services = services.BuildServiceProvider();

            MainPage = new NavigationPage(Services.GetRequiredService<MainPage>());
        }
    }
}

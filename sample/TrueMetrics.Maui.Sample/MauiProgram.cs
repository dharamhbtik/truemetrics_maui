using Microsoft.Extensions.Logging;
using TrueMetrics.Maui;

namespace TrueMetrics.Maui.Sample;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Register TRUE Metrics SDK
        builder.Services.AddTrueMetrics(options =>
        {
            options.ApiKey = "YOUR_API_KEY_HERE";
        });

        // Register pages
        builder.Services.AddTransient<MainPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}

using System.Text;

namespace TrueMetrics.Maui.Sample;

public partial class MainPage : ContentPage
{
    private readonly ITrueMetricsService _metrics;
    private readonly StringBuilder _log = new();
    private bool _initialized;

    public MainPage(ITrueMetricsService metrics)
    {
        InitializeComponent();
        _metrics = metrics;
    }

    private async void OnInitializeClicked(object sender, EventArgs e)
    {
        await RunSafeAsync(async () =>
        {
            await _metrics.InitializeAsync("YOUR_API_KEY_HERE");
            _initialized = true;

            StatusLabel.Text = "Initialized";
            DeviceIdLabel.Text = $"Device ID: {_metrics.DeviceId ?? "—"}";
            InitButton.IsEnabled = false;
            StartButton.IsEnabled = true;
            TrackSimpleButton.IsEnabled = true;
            TrackPropsButton.IsEnabled = true;

            AppendLog("SDK initialized successfully.");
        });
    }

    private async void OnStartSessionClicked(object sender, EventArgs e)
    {
        await RunSafeAsync(async () =>
        {
            await _metrics.StartSessionAsync();
            StatusLabel.Text = "Recording in progress";
            StartButton.IsEnabled = false;
            StopButton.IsEnabled = true;
            AppendLog("Session started.");
        });
    }

    private async void OnStopSessionClicked(object sender, EventArgs e)
    {
        await RunSafeAsync(async () =>
        {
            await _metrics.StopSessionAsync();
            StatusLabel.Text = "Session stopped";
            StopButton.IsEnabled = false;
            StartButton.IsEnabled = true;
            AppendLog("Session stopped.");
        });
    }

    private async void OnTrackSimpleEventClicked(object sender, EventArgs e)
    {
        await RunSafeAsync(async () =>
        {
            await _metrics.TrackEventAsync("ButtonTapped");
            AppendLog("Event tracked: ButtonTapped");
        });
    }

    private async void OnTrackEventWithPropertiesClicked(object sender, EventArgs e)
    {
        await RunSafeAsync(async () =>
        {
            var props = new Dictionary<string, string>
            {
                ["screen"] = "MainPage",
                ["action"] = "demo_tap",
                ["timestamp"] = DateTimeOffset.UtcNow.ToString("o")
            };
            await _metrics.TrackEventAsync("ButtonTappedWithProps", props);
            AppendLog("Event tracked: ButtonTappedWithProps (3 properties)");
        });
    }

    private async void OnSetUserIdClicked(object sender, EventArgs e)
    {
        var userId = UserIdEntry.Text?.Trim();
        if (string.IsNullOrEmpty(userId))
        {
            await DisplayAlert("Validation", "Please enter a user ID.", "OK");
            return;
        }

        await RunSafeAsync(async () =>
        {
            await _metrics.SetUserIdAsync(userId);
            AppendLog($"User ID set: {userId}");
        });
    }

    private async Task RunSafeAsync(Func<Task> action)
    {
        try
        {
            await action();
        }
        catch (PlatformNotSupportedException ex)
        {
            AppendLog($"[Platform] {ex.Message}");
            await DisplayAlert("Not Supported", ex.Message, "OK");
        }
        catch (Exception ex)
        {
            AppendLog($"[Error] {ex.Message}");
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private void AppendLog(string message)
    {
        _log.Insert(0, $"[{DateTime.Now:HH:mm:ss}] {message}\n");
        LogLabel.Text = _log.ToString();
    }
}

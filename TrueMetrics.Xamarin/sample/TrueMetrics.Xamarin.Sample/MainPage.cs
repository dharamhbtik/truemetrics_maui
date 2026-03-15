using System;
using System.Collections.Generic;
using System.Text;
using TrueMetrics.Xamarin;
using Xamarin.Forms;

namespace TrueMetrics.Xamarin.Sample
{
    public class MainPage : ContentPage
    {
        private readonly ITrueMetricsService _metrics;
        private readonly StringBuilder _log = new StringBuilder();

        private readonly Button _initBtn;
        private readonly Button _startBtn;
        private readonly Button _stopBtn;
        private readonly Button _trackSimpleBtn;
        private readonly Button _trackPropsBtn;
        private readonly Button _setUserBtn;
        private readonly Entry _userIdEntry;
        private readonly Label _statusLabel;
        private readonly Label _deviceIdLabel;
        private readonly Label _logLabel;

        public MainPage(ITrueMetricsService metrics)
        {
            _metrics = metrics;
            Title = "TrueMetrics Demo";

            _statusLabel = new Label { Text = "Not initialized", FontSize = 15 };
            _deviceIdLabel = new Label { Text = "Device ID: —", FontSize = 12 };

            _initBtn = MakeButton("Initialize SDK", OnInitializeClicked);
            _startBtn = MakeButton("Start Session", OnStartSessionClicked, enabled: false);
            _stopBtn = MakeButton("Stop Session", OnStopSessionClicked, enabled: false);
            _trackSimpleBtn = MakeButton("Track Event (simple)", OnTrackSimpleClicked, enabled: false);
            _trackPropsBtn = MakeButton("Track Event (with properties)", OnTrackPropsClicked, enabled: false);
            _setUserBtn = MakeButton("Set User ID", OnSetUserIdClicked);

            _userIdEntry = new Entry { Placeholder = "Enter user ID", ReturnType = ReturnType.Done };

            _logLabel = new Label
            {
                Text = "(log output will appear here)",
                FontSize = 12,
                LineBreakMode = LineBreakMode.WordWrap
            };

            Content = new ScrollView
            {
                Content = new StackLayout
                {
                    Padding = new Thickness(24),
                    Spacing = 12,
                    Children =
                    {
                        new Label { Text = "TRUE Metrics SDK", FontSize = 26, FontAttributes = FontAttributes.Bold, HorizontalOptions = LayoutOptions.Center },
                        new Label { Text = "Xamarin.Forms Demo", FontSize = 13, HorizontalOptions = LayoutOptions.Center },

                        new Frame { Content = new StackLayout { Children = { _statusLabel, _deviceIdLabel } }, CornerRadius = 8, Padding = 12 },

                        new Label { Text = "SDK Controls", FontAttributes = FontAttributes.Bold, FontSize = 15 },
                        _initBtn, _startBtn, _stopBtn,

                        new Label { Text = "Event Tracking", FontAttributes = FontAttributes.Bold, FontSize = 15 },
                        _trackSimpleBtn, _trackPropsBtn,

                        new Label { Text = "User Identity", FontAttributes = FontAttributes.Bold, FontSize = 15 },
                        _userIdEntry, _setUserBtn,

                        new Label { Text = "Log", FontAttributes = FontAttributes.Bold, FontSize = 15 },
                        new Frame { Content = _logLabel, CornerRadius = 8, Padding = 10, MinimumHeightRequest = 100 }
                    }
                }
            };
        }

        private async void OnInitializeClicked(object sender, EventArgs e)
        {
            await RunSafeAsync(async () =>
            {
                await _metrics.InitializeAsync("YOUR_API_KEY_HERE");
                _statusLabel.Text = "Initialized";
                _deviceIdLabel.Text = $"Device ID: {_metrics.DeviceId ?? "—"}";
                _initBtn.IsEnabled = false;
                _startBtn.IsEnabled = true;
                _trackSimpleBtn.IsEnabled = true;
                _trackPropsBtn.IsEnabled = true;
                AppendLog("SDK initialized.");
            });
        }

        private async void OnStartSessionClicked(object sender, EventArgs e)
        {
            await RunSafeAsync(async () =>
            {
                await _metrics.StartSessionAsync();
                _statusLabel.Text = "Recording in progress";
                _startBtn.IsEnabled = false;
                _stopBtn.IsEnabled = true;
                AppendLog("Session started.");
            });
        }

        private async void OnStopSessionClicked(object sender, EventArgs e)
        {
            await RunSafeAsync(async () =>
            {
                await _metrics.StopSessionAsync();
                _statusLabel.Text = "Session stopped";
                _stopBtn.IsEnabled = false;
                _startBtn.IsEnabled = true;
                AppendLog("Session stopped.");
            });
        }

        private async void OnTrackSimpleClicked(object sender, EventArgs e)
        {
            await RunSafeAsync(async () =>
            {
                await _metrics.TrackEventAsync("ButtonTapped");
                AppendLog("Event tracked: ButtonTapped");
            });
        }

        private async void OnTrackPropsClicked(object sender, EventArgs e)
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
            var userId = _userIdEntry.Text?.Trim();
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

        private async System.Threading.Tasks.Task RunSafeAsync(Func<System.Threading.Tasks.Task> action)
        {
            try { await action(); }
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
            _logLabel.Text = _log.ToString();
        }

        private static Button MakeButton(string text, EventHandler handler, bool enabled = true) =>
            new Button { Text = text, IsEnabled = enabled, HeightRequest = 44 }
                .Also(b => b.Clicked += handler);
    }

    internal static class ButtonExtensions
    {
        public static T Also<T>(this T obj, Action<T> action) { action(obj); return obj; }
    }
}

using Microsoft.Win32;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using VideoOS.Platform.SDK.Core;
using VideoOS.Platform.SDK.Core.Media;

namespace StableFPSRecorder;

public enum DeviceType { Camera, Microphone, Speaker, Metadata }

public partial class MainWindow : Window
{
    // ── State ──────────────────────────────────────────────────────────────
    private ISession? _session;
    private DeviceType _selectedDeviceType;
    private RawSource? _rawSource;
    private FileStream? _outputFile;
    private long _bytesWritten;
    private TaskCompletionSource<string>? _tokenReady;

    // ── Dependency property helper (used for IsEnabled bindings) ───────────
    public static readonly DependencyProperty IsLoggedInProperty =
        DependencyProperty.Register(nameof(IsLoggedIn), typeof(bool), typeof(MainWindow), new PropertyMetadata(false));

    public bool IsLoggedIn
    {
        get => (bool)GetValue(IsLoggedInProperty);
        private set => SetValue(IsLoggedInProperty, value);
    }

    /// <summary>Convenience property used as default start-time in the XAML binding.</summary>
    public string UtcNowMinus1Hour => DateTimeOffset.UtcNow.AddHours(-1).ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);

    public MainWindow()
    {
        InitializeComponent();
        UpdateCredentialVisibility("Windows Default");
        UpdateStartButton();
    }

    // ── Login ──────────────────────────────────────────────────────────────

    private void LoginTypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (LoginTypeBox.SelectedItem is ComboBoxItem item)
            UpdateCredentialVisibility(item.Content as string ?? string.Empty);
    }

    private void UpdateCredentialVisibility(string loginType)
    {
        if (UsernameLabel is null)
            return;

        bool showCredentials = loginType != "Windows Default";
        UsernameLabel.Visibility = showCredentials ? Visibility.Visible : Visibility.Collapsed;
        UsernameBox.Visibility = showCredentials ? Visibility.Visible : Visibility.Collapsed;
        PasswordLabel.Visibility = showCredentials ? Visibility.Visible : Visibility.Collapsed;
        PasswordBox.Visibility = showCredentials ? Visibility.Visible : Visibility.Collapsed;
    }

    private async void LoginButton_Click(object sender, RoutedEventArgs e)
    {
        LoginButton.IsEnabled = false;
        LoginStatusText.Text = "Logging in…";
        LoginStatusText.Foreground = System.Windows.Media.Brushes.DarkOrange;
        ErrorText.Text = string.Empty;

        try
        {
            var serverUri = new Uri(ServerUriBox.Text.Trim());
            var idpUri = new Uri(serverUri, "idp");
            var serverConfiguration = new ServerConfiguration(serverUri, idpUri);

            IMipCredentials credentials = GetSelectedCredentials();

            _tokenReady = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);

            var newSession = new Session(serverConfiguration, App.Services, credentials);
            newSession.MipTokenCache.OnNewTokenAvailable += OnNewTokenAvailable;
            newSession.MipTokenCache.OnError += OnTokenError;

            await _tokenReady.Task;

            _session = newSession;
            IsLoggedIn = true;
            LoginStatusText.Text = $"Logged in. Session {_session.Id}";
            LoginStatusText.Foreground = System.Windows.Media.Brushes.Green;
        }
        catch (Exception ex)
        {
            LoginStatusText.Text = $"Login failed: {ex.Message}";
            LoginStatusText.Foreground = System.Windows.Media.Brushes.Red;
        }
        finally
        {
            LoginButton.IsEnabled = true;
            UpdateStartButton();
        }
    }

    private IMipCredentials GetSelectedCredentials()
    {
        var loginType = (LoginTypeBox.SelectedItem as ComboBoxItem)?.Content as string ?? "Windows Default";
        return loginType switch
        {
            "Windows" => new WindowsUser(UsernameBox.Text.Trim(), PasswordBox.Password),
            "Basic" => new BasicUser(UsernameBox.Text.Trim(), PasswordBox.Password),
            _ => new DefaultWindowsUser()
        };
    }

    private void OnNewTokenAvailable(string token)
    {
        _tokenReady?.TrySetResult(token);
    }

    private void OnTokenError(Exception ex)
    {
        _tokenReady?.TrySetException(ex);
    }

    // ── Device picker ──────────────────────────────────────────────────────

    private void BrowseDeviceButton_Click(object sender, RoutedEventArgs e)
    {
        if (_session is null)
            return;

        var dialog = new DevicePickerDialog(_session) { Owner = this };
        if (dialog.ShowDialog() == true && dialog.SelectedDevicePath is not null)
        {
            DevicePathBox.Text = dialog.SelectedDevicePath;
            _selectedDeviceType = Enum.Parse<DeviceType>(dialog.SelectedDevicePath.Split('/')[0], ignoreCase: true);
            UpdateStartButton();
        }
    }

    // ── Mode ───────────────────────────────────────────────────────────────

    private void ModeRadio_Changed(object sender, RoutedEventArgs e)
    {
        if (StartTimePanel is null)
            return;

        StartTimePanel.Visibility = RecordedRadio.IsChecked == true
            ? Visibility.Visible
            : Visibility.Collapsed;

        UpdateStartButton();
    }

    // ── Output file ────────────────────────────────────────────────────────

    private void BrowseOutputFileButton_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new SaveFileDialog
        {
            Title = "Choose output file",
            Filter = "Raw data files (*.raw)|*.raw|All files (*.*)|*.*",
            DefaultExt = ".raw"
        };

        if (dialog.ShowDialog(this) == true)
        {
            OutputFileBox.Text = dialog.FileName;
            UpdateStartButton();
        }
    }

    private void OutputFileBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        UpdateStartButton();
    }

    // ── Start / Stop ───────────────────────────────────────────────────────

    private void UpdateStartButton()
    {
        if (StartButton is null)
            return;

        bool deviceChosen = !string.IsNullOrWhiteSpace(DevicePathBox?.Text)
                            && DevicePathBox.Text != "(no device selected)";
        bool outputChosen = !string.IsNullOrWhiteSpace(OutputFileBox?.Text);
        bool startTimeValid = RecordedRadio?.IsChecked != true
                              || TryParseStartTime(out _);

        StartButton.IsEnabled = IsLoggedIn && deviceChosen && outputChosen && startTimeValid && _rawSource is null;
    }

    private bool TryParseStartTime(out DateTimeOffset result)
    {
        return DateTimeOffset.TryParse(StartTimeBox.Text.Trim(), CultureInfo.InvariantCulture, out result);
    }

    private async void StartButton_Click(object sender, RoutedEventArgs e)
    {
        ErrorText.Text = string.Empty;
        try
        {
            var session = _session!;
            var devicePath = DevicePathBox.Text.Trim();
            var outputPath = OutputFileBox.Text.Trim();
            var mode = LiveRadio.IsChecked == true ? RawSourceMode.Live : RawSourceMode.Playback;

            _outputFile = new FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.Read);
            _bytesWritten = 0;
            BytesWrittenText.Text = FormatBytes(0);

            _rawSource = new RawSource(session, mode, devicePath);

            if (mode == RawSourceMode.Playback && TryParseStartTime(out var startTime))
                _rawSource.PlaybackStartTime = startTime;

            _rawSource.DataReady += OnDataReady;
            _rawSource.Error += OnSourceError;

            StartButton.IsEnabled = false;
            StopButton.IsEnabled = true;

            await _rawSource.Start(CancellationToken.None);
        }
        catch (Exception ex)
        {
            ErrorText.Text = ex.Message;
            await CleanupSourceAsync();
            UpdateStartButton();
        }
    }

    private async void StopButton_Click(object sender, RoutedEventArgs e)
    {
        StopButton.IsEnabled = false;
        await CleanupSourceAsync();
        UpdateStartButton();
    }

    private async Task CleanupSourceAsync()
    {
        if (_rawSource is not null)
        {
            _rawSource.DataReady -= OnDataReady;
            _rawSource.Error -= OnSourceError;
            try
            {
                await _rawSource.Stop();
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                ErrorText.Text = ex.Message;
            }

            await _rawSource.DisposeAsync();
            _rawSource = null;
        }

        if (_outputFile is not null)
        {
            _outputFile.Flush();
            _outputFile.Close();
            _outputFile = null;
        }
    }

    // ── Data handling ──────────────────────────────────────────────────────

    private static ReadOnlySpan<byte> MetadataOpen => "{"u8;
    private static ReadOnlySpan<byte> MetadataClose => "}"u8;
    private static readonly System.Text.RegularExpressions.Regex UtcTimeAttributeRegex =
        new(@"UtcTime=""[^""]*""", System.Text.RegularExpressions.RegexOptions.Compiled);

    private void OnDataReady(object? sender, RawData e)
    {
        var data = e.Data;
        if (_selectedDeviceType == DeviceType.Metadata)
        {
            // StableFPS driver expects the UtcTime attribute to be replaced with a placeholder string, so that it can be filled in with the actual frame time later.
            var xml = System.Text.Encoding.UTF8.GetString(data);
            var replaced = UtcTimeAttributeRegex.Replace(xml, @"UtcTime=""FRAME_TIME_HOLDER""");
            var replacedBytes = System.Text.Encoding.UTF8.GetBytes(replaced);

            _outputFile?.Write(MetadataOpen);
            _outputFile?.Write(replacedBytes);
            _outputFile?.Write(MetadataClose);
            _bytesWritten += MetadataOpen.Length + replacedBytes.Length + MetadataClose.Length;
        }
        else
        {
            _outputFile?.Write(data);
            _bytesWritten += data.Length;
        }

        Dispatcher.InvokeAsync(() =>
        {
            BytesWrittenText.Text = FormatBytes(_bytesWritten);
        });
    }

    private void OnSourceError(object? sender, Exception e)
    {
        Dispatcher.InvokeAsync(() =>
        {
            ErrorText.Text = e.Message;
        });
    }

    // ── Helpers ────────────────────────────────────────────────────────────

    private static string FormatBytes(long bytes)
    {
        return bytes switch
        {
            < 1024 => $"{bytes} B",
            < 1024 * 1024 => $"{bytes / 1024.0:F1} KB",
            < 1024L * 1024 * 1024 => $"{bytes / (1024.0 * 1024):F2} MB",
            _ => $"{bytes / (1024.0 * 1024 * 1024):F3} GB"
        };
    }
}

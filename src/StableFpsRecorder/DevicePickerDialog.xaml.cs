using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using VideoOS.Platform.SDK.Core;
using VideoOS.Platform.SDK.Core.Configuration;
using VideoOS.Platform.SDK.Core.Configuration.Items;
using Task = System.Threading.Tasks.Task;

namespace StableFPSRecorder;

public partial class DevicePickerDialog : Window
{
    private readonly ISession _session;
    private readonly ObservableCollection<DeviceTreeNode> _roots = [];

    /// <summary>The device path of the selected device, or <see langword="null"/> if none was chosen.</summary>
    public string? SelectedDevicePath { get; private set; }

    public DevicePickerDialog(ISession session)
    {
        _session = session;
        InitializeComponent();
        DeviceTree.DataContext = _roots;
        Loaded += async (_, _) => await LoadRootsAsync();
    }

    // ── Initial load: create the 4 root category nodes only ──────────────

    private async Task LoadRootsAsync()
    {
        try
        {
            var configuration = new ConfigurationService(_session);

            var cameraGroupsTask = configuration.Get<CameraGroup>();
            var micGroupsTask = configuration.Get<MicrophoneGroup>();
            var speakerGroupsTask = configuration.Get<SpeakerGroup>();
            var metaGroupsTask = configuration.Get<MetadataGroup>();

            await Task.WhenAll(cameraGroupsTask, micGroupsTask, speakerGroupsTask, metaGroupsTask);

            var cameraRoot = new DeviceTreeNode("Cameras");
            foreach (var group in cameraGroupsTask.Result)
                cameraRoot.Children.Add(CreateCameraGroupNode(group));

            var micRoot = new DeviceTreeNode("Microphones");
            foreach (var group in micGroupsTask.Result)
                micRoot.Children.Add(CreateMicrophoneGroupNode(group));

            var speakerRoot = new DeviceTreeNode("Speakers");
            foreach (var group in speakerGroupsTask.Result)
                speakerRoot.Children.Add(CreateSpeakerGroupNode(group));

            var metaRoot = new DeviceTreeNode("Metadata Devices");
            foreach (var group in metaGroupsTask.Result)
                metaRoot.Children.Add(CreateMetadataGroupNode(group));

            _roots.Add(cameraRoot);
            _roots.Add(micRoot);
            _roots.Add(speakerRoot);
            _roots.Add(metaRoot);
        }
        catch (Exception ex)
        {
            LoadingText.Text = $"Error loading devices: {ex.Message}";
            LoadingText.Foreground = System.Windows.Media.Brushes.Red;
            LoadingText.Visibility = Visibility.Visible;
            return;
        }

        LoadingText.Visibility = Visibility.Collapsed;
    }

    // ── Factory methods: create a group node with a lazy Loader ──────────

    private static DeviceTreeNode CreateCameraGroupNode(CameraGroup group)
    {
        var node = new DeviceTreeNode(group.Name ?? group.Id.ToString());
        node.Children.Add(DeviceTreeNode.NewPlaceholder());
        node.Loader = async () =>
        {
            foreach (var device in await group.GetChildrenAsync<Camera>())
                node.Children.Add(new DeviceTreeNode(device.Name ?? device.Id.ToString(), $"camera/{device.Id}"));
            foreach (var sub in await group.GetChildrenAsync<CameraGroup>())
                node.Children.Add(CreateCameraGroupNode(sub));
        };
        return node;
    }

    private static DeviceTreeNode CreateMicrophoneGroupNode(MicrophoneGroup group)
    {
        var node = new DeviceTreeNode(group.Name ?? group.Id.ToString());
        node.Children.Add(DeviceTreeNode.NewPlaceholder());
        node.Loader = async () =>
        {
            foreach (var device in await group.GetChildrenAsync<Microphone>())
                node.Children.Add(new DeviceTreeNode(device.Name ?? device.Id.ToString(), $"microphone/{device.Id}"));
            foreach (var sub in await group.GetChildrenAsync<MicrophoneGroup>())
                node.Children.Add(CreateMicrophoneGroupNode(sub));
        };
        return node;
    }

    private static DeviceTreeNode CreateSpeakerGroupNode(SpeakerGroup group)
    {
        var node = new DeviceTreeNode(group.Name ?? group.Id.ToString());
        node.Children.Add(DeviceTreeNode.NewPlaceholder());
        node.Loader = async () =>
        {
            foreach (var device in await group.GetChildrenAsync<Speaker>())
                node.Children.Add(new DeviceTreeNode(device.Name ?? device.Id.ToString(), $"speaker/{device.Id}"));
            foreach (var sub in await group.GetChildrenAsync<SpeakerGroup>())
                node.Children.Add(CreateSpeakerGroupNode(sub));
        };
        return node;
    }

    private static DeviceTreeNode CreateMetadataGroupNode(MetadataGroup group)
    {
        var node = new DeviceTreeNode(group.Name ?? group.Id.ToString());
        node.Children.Add(DeviceTreeNode.NewPlaceholder());
        node.Loader = async () =>
        {
            foreach (var device in await group.GetChildrenAsync<Metadata>())
                node.Children.Add(new DeviceTreeNode(device.Name ?? device.Id.ToString(), $"metadata/{device.Id}"));
            foreach (var sub in await group.GetChildrenAsync<MetadataGroup>())
                node.Children.Add(CreateMetadataGroupNode(sub));
        };
        return node;
    }

    // ── Lazy expand handler ───────────────────────────────────────────────

    private async void TreeViewItem_Expanded(object sender, RoutedEventArgs e)
    {
        if (e.OriginalSource is not TreeViewItem tvi || tvi.DataContext is not DeviceTreeNode node)
            return;

        if (node.IsLoaded || node.Loader is null)
            return;

        node.IsLoaded = true;
        node.Children.Clear();

        try
        {
            await node.Loader();
        }
        catch (Exception ex)
        {
            node.Children.Add(new DeviceTreeNode($"Error: {ex.Message}"));
        }

        e.Handled = true;
    }

    // ── Selection / OK ────────────────────────────────────────────────────

    private void DeviceTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
        if (e.NewValue is DeviceTreeNode node && node.IsDevice)
        {
            SelectedDevicePath = node.DevicePath;
            OkButton.IsEnabled = true;
        }
        else
        {
            SelectedDevicePath = null;
            OkButton.IsEnabled = false;
        }
    }

    private void OkButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
    }
}

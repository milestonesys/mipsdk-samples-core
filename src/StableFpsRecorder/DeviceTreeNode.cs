using System.Collections.ObjectModel;

namespace StableFPSRecorder;

/// <summary>
/// A node in the device picker tree. Leaf nodes represent selectable devices;
/// non-leaf nodes represent groups.
/// </summary>
public sealed class DeviceTreeNode
{
    public string DisplayName { get; }

    /// <summary>
    /// The device path used by <see cref="VideoOS.Platform.SDK.Core.Media.RawSource"/>,
    /// e.g. <c>"camera/&lt;guid&gt;"</c>. <see langword="null"/> for group nodes.
    /// </summary>
    public string? DevicePath { get; }

    public bool IsDevice => DevicePath is not null;

    public ObservableCollection<DeviceTreeNode> Children { get; } = [];

    /// <summary>
    /// Called the first time this node is expanded to populate <see cref="Children"/>.
    /// <see langword="null"/> for leaf (device) nodes.
    /// </summary>
    public Func<Task>? Loader { get; set; }

    /// <summary><see langword="true"/> once <see cref="Loader"/> has been invoked.</summary>
    public bool IsLoaded { get; set; }

    public DeviceTreeNode(string displayName, string? devicePath = null)
    {
        DisplayName = displayName;
        DevicePath = devicePath;
    }

    /// <summary>Creates a temporary placeholder child shown while children are loading.</summary>
    public static DeviceTreeNode NewPlaceholder() => new("Loading\u2026");
}

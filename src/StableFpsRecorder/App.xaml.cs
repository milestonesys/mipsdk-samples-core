using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using VideoOS.Platform.SDK.Core.Extensions;

namespace StableFPSRecorder;

public partial class App : Application
{
    public static IServiceProvider Services { get; private set; } = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var services = new ServiceCollection();
        services.AddMipServices();
        Services = services.BuildServiceProvider();
    }
}

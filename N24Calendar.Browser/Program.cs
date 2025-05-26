using System.Collections.Generic;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Browser;
using N24Calendar;

internal sealed partial class Program
{
    private static Task Main(string[] args) => BuildAvaloniaApp(args)
            .WithInterFont()
            .StartBrowserAppAsync("out", new BrowserPlatformOptions() 
			{
				RenderingMode = new List<BrowserRenderingMode> { BrowserRenderingMode.WebGL1 },
			});

    public static AppBuilder BuildAvaloniaApp(string[] args)
        => AppBuilder.Configure<App>(() => new App(args));
}
using Avalonia;
using Avalonia.ReactiveUI;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Guruguru
{
    class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args)
        {
            try
            {
                BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args);
            }
            catch (Exception ex)
            {
                File.WriteAllText($"logs-{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.log", ex.ToString());
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    _ = MessageBox(IntPtr.Zero, $"The application crashed, more information were saved at {Directory.GetCurrentDirectory()}", ex.GetType().ToString(), 0x00000000L);
                }
            }
        }

        [DllImport("User32.dll")]
        public static extern int MessageBox(IntPtr handle, string text, string caption, long type);

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace()
                .UseReactiveUI();
    }
}

using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Guruguru.ViewModels;
using ReactiveUI;
using System.Reactive;
using System.Threading.Tasks;

namespace Guruguru.Views
{
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            AvaloniaXamlLoader.Load(this);
#if DEBUG
            this.AttachDevTools();
#endif
            this.WhenActivated(_ =>
            {
                ViewModel.GetFolder.RegisterHandler(GetFolder);
            });
        }

        private async Task GetFolder(InteractionContext<Unit, string> ctx)
        {
            var dialog = new OpenFolderDialog();
            var folder = await dialog.ShowAsync((Window)VisualRoot).ConfigureAwait(false);
            ctx.SetOutput(folder);
        }
    }
}

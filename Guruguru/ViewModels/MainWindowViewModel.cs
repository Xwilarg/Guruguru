using ReactiveUI;
using System.Reactive;
using System.Windows.Input;

namespace Guruguru.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        public MainWindowViewModel()
        {
            SelectFolder = ReactiveCommand.CreateFromTask(async () =>
            {
                GetFolder.Handle(Unit.Default).Subscribe(Observer.Create<string>(folder =>
                {
                }));
            });
        }

        public Interaction<Unit, string> GetFolder { get; } = new();
        public ICommand SelectFolder { get; }
    }
}

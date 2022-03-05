using Avalonia.Media.Imaging;
using Avalonia.Threading;
using ReactiveUI;
using System;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Threading;
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
                    _images = Directory.GetFiles(folder);
                }));
            });

            new Thread(new ThreadStart(ChangeImage)).Start();
        }

        public void ChangeImage()
        {
            while (Thread.CurrentThread.IsAlive)
            {
                if (_images.Any())
                {
                    if (_index >= _images.Length - 1)
                    {
                        _index = 0;
                    }
                    else
                    {
                        _index++;
                    }

                    Dispatcher.UIThread.Post(() =>
                    {
                        GetWidth.Handle(Unit.Default).Subscribe(Observer.Create<double>(width =>
                        {
                            using var stream = File.OpenRead(_images[_index]);
                            ImageSource = Bitmap.DecodeToWidth(stream, (int)width);
                        }));
                    });
                }
                Thread.Sleep(2000);
            }
        }

        private string[] _images = Array.Empty<string>();
        private int _index;

        public Interaction<Unit, string> GetFolder { get; } = new();
        public Interaction<Unit, double> GetWidth { get; } = new();
        public ICommand SelectFolder { get; }

        private Bitmap _imageSource;
        public Bitmap ImageSource
        {
            get => _imageSource;
            set => this.RaiseAndSetIfChanged(ref _imageSource, value);
        }
    }
}

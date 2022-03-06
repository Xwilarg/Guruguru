using Avalonia.Media.Imaging;
using Avalonia.Threading;
using ReactiveUI;
using System;
using System.IO;
using System.Linq;
using System.Media;
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

        public void Close()
        {
            _isAlive = false;
        }

        private void ChangeImage()
        {
            while (Thread.CurrentThread.IsAlive && _isAlive)
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
                    if (_doesPlayAudio)
                    {
                        _bip.Play();
                    }
                }
                for (int i = 0; i < Delay && _isAlive; i++)
                {
                    Thread.Sleep(1000);
                }
            }
        }

        private bool _isAlive = true;
        private string[] _images = Array.Empty<string>();
        private int _index;

        private SoundPlayer _bip = new("Assets/Bip.wav");

        public Interaction<Unit, string> GetFolder { get; } = new();
        public Interaction<Unit, double> GetWidth { get; } = new();
        public ICommand SelectFolder { get; }

        private int _delay = 1;
        public int Delay
        {
            set
            {
                this.RaiseAndSetIfChanged(ref _delay, value > 1 ? value : 1);
            }
            get => _delay;
        }

        private Bitmap _imageSource;
        public Bitmap ImageSource
        {
            get => _imageSource;
            set => this.RaiseAndSetIfChanged(ref _imageSource, value);
        }

        private bool _doesPlayAudio = true;
        public bool DoesPlayAudio
        {
            get => _doesPlayAudio;
            set => this.RaiseAndSetIfChanged(ref _doesPlayAudio, value);
        }
    }
}

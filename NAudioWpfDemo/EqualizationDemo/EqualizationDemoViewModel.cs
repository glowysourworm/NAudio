using System;
using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Win32;
using NAudio.Extras;
using NAudio.Wave;
using NAudioWpfDemo.ViewModel;

namespace NAudioWpfDemo.EqualizationDemo
{
    class EqualizationDemoViewModel : ViewModelBase, IDisposable
    {
        private AudioFileReader reader;
        private IWavePlayer player;
        private Equalizer equalizer;
        private string selectedFile;
        private readonly EqualizerBand[] bands;
        
        public ICommand OpenFileCommand { get; }
        public ICommand PlayCommand { get; }
        public ICommand PauseCommand { get; }
        public ICommand StopCommand { get; }

        public EqualizationDemoViewModel()
        {
            PlayCommand = new DelegateCommand(Play);
            OpenFileCommand = new DelegateCommand(OpenFile);
            StopCommand = new DelegateCommand(Stop);
            PauseCommand = new DelegateCommand(Pause);
            bands = new EqualizerBand[]
            {
                new EqualizerBand(100, 0, 0.8f, 2),
                new EqualizerBand(200, 0, 0.8f, 2),
                new EqualizerBand(400, 0, 0.8f, 2),
                new EqualizerBand(800, 0, 0.8f, 2),
                new EqualizerBand(1200, 0, 0.8f, 2),
                new EqualizerBand(240, 0, 0.8f, 2),
                new EqualizerBand(4800, 0, 0.8f, 2),
                new EqualizerBand(9600, 0, 0.8f, 2),
            };
            this.PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            equalizer?.Update();
        }

        public float MinimumGain => -30;

        public float MaximumGain => 30;

        public float Band1
        {
            get => bands[0].GetGain();
            set
            {
                if (bands[0].GetGain() != value)
                {
                    bands[0].UpdateParameters(value);
                    OnPropertyChanged("Band1");
                }
            }
        }

        public float Band2
        {
            get => bands[1].GetGain();
            set
            {
                if (bands[1].GetGain() != value)
                {
                    bands[1].UpdateParameters(value);
                    OnPropertyChanged("Band2");
                }
            }
        }

        public float Band3
        {
            get => bands[2].GetGain();
            set
            {
                if (bands[2].GetGain() != value)
                {
                    bands[2].UpdateParameters(value);
                    OnPropertyChanged("Band3");
                }
            }
        }

        public float Band4
        {
            get => bands[3].GetGain();
            set
            {
                if (bands[3].GetGain() != value)
                {
                    bands[3].UpdateParameters(value);
                    OnPropertyChanged("Band4");
                }
            }
        }

        public float Band5
        {
            get => bands[4].GetGain();
            set
            {
                if (bands[4].GetGain() != value)
                {
                    bands[4].UpdateParameters(value);
                    OnPropertyChanged("Band5");
                }
            }
        }

        public float Band6
        {
            get => bands[5].GetGain();
            set
            {
                if (bands[5].GetGain() != value)
                {
                    bands[5].UpdateParameters(value);
                    OnPropertyChanged("Band6");
                }
            }
        }


        public float Band7
        {
            get => bands[6].GetGain();
            set
            {
                if (bands[6].GetGain() != value)
                {
                    bands[6].UpdateParameters(value);
                    OnPropertyChanged("Band7");
                }
            }
        }

        public float Band8
        {
            get => bands[7].GetGain();
            set
            {
                if (bands[7].GetGain() != value)
                {
                    bands[7].UpdateParameters(value);
                    OnPropertyChanged("Band8");
                }
            }
        }

        private void Pause()
        {
            player?.Pause();
        }

        private void OpenFile()
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All Supported Files (*.wav;*.mp3)|*.wav;*.mp3|All Files (*.*)|*.*";
            bool? result = openFileDialog.ShowDialog();
            if (result.HasValue && result.Value)
            {
                selectedFile = openFileDialog.FileName;
                reader = new AudioFileReader(selectedFile);
                equalizer = new Equalizer(reader, bands);
                player = new WaveOutEvent();
                player.Init(equalizer);
            }
        }

        private void Play()
        {
            if (selectedFile == null)
            {
                OpenFile();
            }
            if (selectedFile != null)
            {
                player.Play();
            }
        }

        private void Stop()
        {
            player?.Stop();
        }

        public void Dispose()
        {
            player?.Dispose();
            reader?.Dispose();
        }
    }
}

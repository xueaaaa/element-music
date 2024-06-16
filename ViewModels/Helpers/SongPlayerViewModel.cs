using ElementMusic.Models.ElementAPI;
using System.Windows.Media;
using System.Windows.Threading;

namespace ElementMusic.ViewModels.Helpers
{
    public partial class SongPlayerViewModel : ObservableObject
    {
        private readonly MediaPlayer _mediaPlayer 
            = new MediaPlayer();


        private Song _song;
        public Song Song
        {
            get => _song;
            set
            {
                _song = value;
                SongLoaded = true;
                OnPropertyChanged(nameof(Song));
            }
        }

        [ObservableProperty]
        private bool _songLoaded;
        [ObservableProperty]
        private bool _playing;
        [ObservableProperty]
        private double _playingProgress;
        [ObservableProperty]
        private string _playingProgressLabel = "--:-- / --:--";
        [ObservableProperty]
        private int _volume;
        [ObservableProperty]
        private bool _isVolumeMenuOpen;

        private bool _paused;
        private bool _ignoreChange;

        public SongPlayerViewModel()
        {
            Volume = (int)(_mediaPlayer.Volume * 100);

            _mediaPlayer.MediaOpened += (_, _) =>
            {
                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += Timer_Tick;
                timer.Start();
            };
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            _ignoreChange = true;
            PlayingProgress = _mediaPlayer.Position.TotalSeconds / _mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds * 100;
            _ignoreChange = false;
            PlayingProgressLabel = $"{_mediaPlayer.Position:mm\\:ss} / {_mediaPlayer.NaturalDuration.TimeSpan:mm\\:ss}";
        }

        public void PlayingProgressChanged()
        {
            if (_ignoreChange) return;

            double newProgress = (PlayingProgress * _mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds) / 100;
            _mediaPlayer.Position = TimeSpan.FromSeconds(newProgress);
        }

        public void VolumeChanged() =>
            _mediaPlayer.Volume = (double)Volume / 100;

        [RelayCommand]
        private void Play()
        {
            if (!_paused)
                _mediaPlayer.Open(new Uri($"https://elemsocial.com/Content/Music/Files/{Song.File}"));
            _mediaPlayer.Play();
            Playing = true;
        }

        [RelayCommand]
        private void Pause() 
        {
            _mediaPlayer.Pause();
            Playing = false;
            _paused = true;
        }

        [RelayCommand]
        private void OpenVolumeMenu() 
        {
            if (!IsVolumeMenuOpen)
                IsVolumeMenuOpen = true;       
        }
    }
}

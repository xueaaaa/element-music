using ElementMusic.Models.ElementAPI;
using System.Windows.Media;
using System.Windows.Threading;
using MessageBox = Wpf.Ui.Controls.MessageBox;

namespace ElementMusic.ViewModels.Helpers
{
    public partial class SongPlayerViewModel : ObservableObject
    {
        private readonly MediaPlayer _mediaPlayer 
            = new MediaPlayer();

        private Song _currentSong;
        public Song CurrentSong
        {
            get => _currentSong;
            set
            {
                _currentSong = value;
                SongLoaded = true;
                OnPropertyChanged(nameof(CurrentSong));
            }
        }

        private Queue<Song> _songQueue
            = new Queue<Song>();

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

            if(PlayingProgress >= 100 && _songQueue.TryPeek(out var song))
            {
                CurrentSong = song;
                Play();
            }
        }

        public void AddToQueue(Song song) =>
            _songQueue.Enqueue(song);

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
                _mediaPlayer.Open(new Uri($"https://elemsocial.com/Content/Music/Files/{CurrentSong.File}"));
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

        [RelayCommand]
        private async void ShowInfo()
        {
            var msgBox = new MessageBox
            {
                Title = Application.Current.Resources["SongInfoHeader"].ToString(),
                Content = $"{Application.Current.Resources["SongTitleLabel"]}: {CurrentSong.Title}" +
                $"\n{Application.Current.Resources["SongArtistLabel"]}: {CurrentSong.Artist}" +
                $"\n{Application.Current.Resources["ReleaseYearLabel"]}: {CurrentSong.ReleaseYear}" +
                $"\n{Application.Current.Resources["BitrateLabel"]}: {CurrentSong.Bitrate}" +
                $"\n{Application.Current.Resources["FormatLabel"]}: {CurrentSong.AudioFormat}",
                CloseButtonText = Application.Current.Resources["CloseButton"].ToString() ?? string.Empty
            };

            await msgBox.ShowDialogAsync();
        }
    }
}

using ElementMusic.Models.ElementAPI;
using ElementMusic.ViewModels.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using MessageBox = Wpf.Ui.Controls.MessageBox;

namespace ElementMusic.ViewModels.Helpers
{
    public partial class SongPlayerViewModel : ObservableObject
    {
        private readonly MediaPlayer _mediaPlayer
            = new();
        private readonly DispatcherTimer _timer =
            new();

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

        private LinkedList<Song> _playedSongs =
            new();

        [ObservableProperty]
        private bool _songLoaded;
        [ObservableProperty]
        private bool _playing;
        [ObservableProperty]
        private double _playingProgress;
        [ObservableProperty]
        private string _playedLabel = "--:--";
        [ObservableProperty]
        private string _durationLabel = "--:--";
        [ObservableProperty]
        private int _volume;
        [ObservableProperty]
        private bool _backwardEnabled;
        [ObservableProperty]
        private bool _forwardEnabled;

        [ObservableProperty]
        private FlyoutViewModel _flyoutViewModel =
            new();

        private bool _paused;
        private bool _ignoreChange;

        public SongPlayerViewModel()
        {
            Volume = (int)(_mediaPlayer.Volume * 100);

            _timer.Interval = TimeSpan.FromMilliseconds(1);
            _timer.Tick += Timer_Tick;

            _mediaPlayer.MediaOpened += (_, _) =>
            {
                _timer.Start();
            };
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            _ignoreChange = true;
            PlayingProgress = _mediaPlayer.Position.TotalSeconds / _mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds * 100;
            _ignoreChange = false;
            PlayedLabel = $"{_mediaPlayer.Position:mm\\:ss}";
            DurationLabel = $"-{_mediaPlayer.NaturalDuration.TimeSpan - _mediaPlayer.Position:mm\\:ss}";

            var currentSongNode = _playedSongs.Find(CurrentSong);

            BackwardEnabled = currentSongNode.Previous != null;
            ForwardEnabled = currentSongNode.Next != null;

            if (PlayingProgress >= 100 && currentSongNode.Next?.Value != null)
            {
                Skip(currentSongNode.Next.Value, false);
            }
            else if (PlayingProgress >= 100 && (_playedSongs == null || currentSongNode.Next?.Value == null)) 
                Stop();
        }

        public void AddToQueue(Song song)
        {
            if (!_playedSongs.Contains(song))
            {
                _playedSongs.AddLast(song);
                ForwardEnabled = true;
            }
            else App.GetService<MainWindowViewModel>().InfoBarViewModel
                    .ErrorTemplate(Application.Current.Resources["AlreadyInQueue"].ToString());
        }

        public void PlayingProgressChanged()
        {
            if (_ignoreChange) return;

            double newProgress = (PlayingProgress * _mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds) / 100;
            _mediaPlayer.Position = TimeSpan.FromSeconds(newProgress);
        }

        public void VolumeChanged() =>
            _mediaPlayer.Volume = (double)Volume / 100;

        public LinkedList<Song>? GetPlayedSongs() => _playedSongs;

        public void StartFromNew(Song song)
        {
            Stop();
            if (_playedSongs.Contains(song))
                _playedSongs.Remove(song);
            _playedSongs.AddLast(song);
            CurrentSong = song;
            Play();
        }

        public void Skip(Song song, bool addToPlayedSongs = true)
        {
            _timer.Stop();
            _mediaPlayer.Stop();
            if(addToPlayedSongs) 
                _playedSongs.AddLast(song);
            CurrentSong = song;
            Play();
        }

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
        private void Stop()
        {
            _timer.Stop();
            _mediaPlayer.Stop();
            CurrentSong = null;
            SongLoaded = false;
            Playing = false;
            _paused = false;
        }

        [RelayCommand]
        private void GoBackwardSong()
        {
            if (_playedSongs.Count > 1)
            {
                var prev = _playedSongs.Find(CurrentSong)?.Previous?.Value;
                Skip(prev, false);
            }
            else BackwardEnabled = false;
        }

        [RelayCommand]
        private void GoForwardSong()
        {
            if (_playedSongs.Find(CurrentSong)?.Next != null)
            {
                var next = _playedSongs.Find(CurrentSong)?.Next?.Value;
                Skip(next, false);
            }
            else BackwardEnabled = false;
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

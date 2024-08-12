using ElementMusic.Models.ElementAPI;
using ElementMusic.Models.MusixmatchAPI;
using ElementMusic.ViewModels.Pages;
using ElementMusic.ViewModels.Windows;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Windows.Media;
using System.Windows.Threading;
using MessageBox = Wpf.Ui.Controls.MessageBox;

namespace ElementMusic.ViewModels.Helpers
{
    public partial class SongPlayerViewModel : ObservableObject
    {
        private const string DURATION_LABEL_PLACEHOLDER =
            "--:--";

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
                if(value != null) LoadLyrics(value);
                OnPropertyChanged(nameof(CurrentSong));
            }
        }

        private LinkedList<Song> _playedSongs =
            new();

        public ObservableCollection<Song> PlayedSongsPublic
        {
            get => new(_playedSongs.Reverse());
        }

        [ObservableProperty]
        private LyricsDisplayViewModel _lyricsDisplayViewModel
            = new();

        [ObservableProperty]
        private bool _songLoaded;
        [ObservableProperty]
        private bool _playing;
        [ObservableProperty]
        private double _playingProgress;
        [ObservableProperty]
        private string _playedLabel
            = DURATION_LABEL_PLACEHOLDER;
        [ObservableProperty]
        private string _durationLabel 
            = DURATION_LABEL_PLACEHOLDER;
        [ObservableProperty]
        private int _volume;
        [ObservableProperty]
        private bool _backwardEnabled;
        [ObservableProperty]
        private bool _forwardEnabled;
        [ObservableProperty]
        private bool _enabled;
        [ObservableProperty]
        private bool _repeatSongs;

        [ObservableProperty]
        private FlyoutViewModel _volumeFlyoutViewModel =
            new();
        [ObservableProperty]
        private FlyoutViewModel _queueFlyoutViewModel =
            new();

        private bool _paused;
        private bool _ignoreChange;

        public SongPlayerViewModel()
        {
            Volume = Properties.Settings.Default.Volume;
            VolumeChanged();

            _timer.Interval = TimeSpan.FromMilliseconds(1);
            _timer.Tick += Timer_Tick;

            _mediaPlayer.MediaOpened += (_, _) =>
            {
                _timer.Start();
                Enabled = true;
            };
        }

        private async void LoadLyrics(Song song)
        {
            LyricsDisplayViewModel.Lyrics = null;
            LyricsDisplayViewModel.NoLyrics = false;
            LyricsDisplayViewModel.LyricsScroller.ScrollToTop();

            var resp = await App.MusixmatchAPISender.SendRequest("getLyricsMusix.php", HttpMethod.Get, new Dictionary<string, object>
            {
                { "t", song.Title },
                { "a", song.Artist },
                { "d", song.Duration },
                { "type", "alternative" }
            });
            var content = await resp.Content.ReadAsStringAsync();

            if (content != null && !content.Contains("error"))
                LyricsDisplayViewModel.Lyrics = LyricsParser.Parse(content.Split('\n'));
            else LyricsDisplayViewModel.Lyrics = new List<Lyrics>();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            _ignoreChange = true;
            PlayingProgress = _mediaPlayer.Position.TotalSeconds / _mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds * 100;
            _ignoreChange = false;
            PlayedLabel = $"{_mediaPlayer.Position:mm\\:ss}";
            DurationLabel = $"-{_mediaPlayer.NaturalDuration.TimeSpan - _mediaPlayer.Position:mm\\:ss}";
            LyricsDisplayViewModel.CurrentPos = _mediaPlayer.Position;

            var currentSongNode = _playedSongs.Find(CurrentSong);

            BackwardEnabled = currentSongNode.Previous != null;
            ForwardEnabled = currentSongNode.Next != null;

            if (PlayingProgress >= 100 && currentSongNode.Next?.Value != null)
            {
                Skip(currentSongNode.Next.Value, false);
            }
            else if (PlayingProgress >= 100 && (_playedSongs == null || currentSongNode.Next?.Value == null))
            {
                if (RepeatSongs)
                    Skip(_playedSongs.First.Value, false);
                else
                {
                    Stop();
                    LyricsDisplayViewModel.Visibility = Visibility.Collapsed;
                }
            }
        }

        public void MoveToTime(TimeSpan timeSpan) =>
            _mediaPlayer.Position = timeSpan;

        public void AddToQueue(Song song)
        {
            if (!_playedSongs.Contains(song))
            {
                _playedSongs.AddLast(song);
                OnPropertyChanged(nameof(PlayedSongsPublic));
                ForwardEnabled = true;
            }
            else App.GetService<MainWindowViewModel>().InfoBarViewModel
                    .ErrorTemplate($"{Application.Current.Resources["AlreadyInQueue"]}: {song.Title}");
        }

        public void PlayingProgressChanged()
        {
            if (_ignoreChange) return;

            double newProgress = (PlayingProgress * _mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds) / 100;
            _mediaPlayer.Position = TimeSpan.FromSeconds(newProgress);
        }

        public void VolumeChanged(bool save = false)
        {
            _mediaPlayer.Volume = (double)Volume / 100;
            App.GetService<SettingsViewModel>().Volume = Volume;
            if (save)
                Properties.Settings.Default.Volume = Volume;
        }

        public void StartFromNew(Song song)
        {
            var startTimer = song == CurrentSong;

            Stop();
            if (_playedSongs.Contains(song))
                _playedSongs.Remove(song);
            _playedSongs.AddLast(song);
            OnPropertyChanged(nameof(PlayedSongsPublic));
            CurrentSong = song;
            Play(startTimer);
        }

        [Obsolete("This method is outdated and should be removed with future updates.")]
        public void Skip(Song song, bool addToPlayedSongs = true)
        {
            _timer.Stop();
            _mediaPlayer.Stop();
            if (addToPlayedSongs)
            {
                _playedSongs.AddLast(song);
                OnPropertyChanged(nameof(PlayedSongsPublic));
            }
            CurrentSong = song;
            Play(false);
        }

        [RelayCommand]
        private void ClearQueue()
        {
            _playedSongs.Clear();
            if (CurrentSong != null)
                _playedSongs.AddLast(CurrentSong);
            OnPropertyChanged(nameof(PlayedSongsPublic));
        }

        [RelayCommand]
        private void Play(object startTimer)
        {
            if (!_paused)
            {
                Enabled = false;
                PlayedLabel = DURATION_LABEL_PLACEHOLDER;
                DurationLabel = DURATION_LABEL_PLACEHOLDER;
                _mediaPlayer.Open(new Uri($"https://elemsocial.com/Content/Music/Files/{CurrentSong.File}"));
            }
            _mediaPlayer.Play();
            if (Convert.ToBoolean(startTimer)) _timer.Start();
            Playing = true;
        }

        [RelayCommand]
        private void Pause()
        {
            _mediaPlayer.Pause();
            _timer.Stop();
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
            if (_playedSongs.Find(CurrentSong)?.Previous != null)
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
        private void OpenLyrics() => 
            LyricsDisplayViewModel.Visibility = LyricsDisplayViewModel.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

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

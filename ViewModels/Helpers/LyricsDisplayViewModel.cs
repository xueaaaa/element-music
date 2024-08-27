using ElementMusic.Models.MusixmatchAPI;
using ElementMusic.ViewModels.Pages;
using System.Windows.Controls;

namespace ElementMusic.ViewModels.Helpers
{
    public partial class LyricsDisplayViewModel : ObservableObject
    {
        [ObservableProperty]
        private List<Lyrics> _lyrics;

        private TimeSpan _currentPos;
        public TimeSpan CurrentPos
        {
            get => _currentPos;
            set
            {
                _currentPos = value;
                OnPropertyChanged(nameof(CurrentPos));

                RemainToStartBarVisibility = Visibility.Collapsed;
                if (Lyrics != null && Lyrics.Count > 0)
                {
                    NoLyrics = false;
                    if ((Visibility)App.GetService<SettingsViewModel>().MiniLyricsDisplayVisibility.Tag == Visibility.Visible)
                        MiniLyricsPanelVisibility = Visibility.Visible;

                    var first = Lyrics.First();
                    if (_currentPos < first.TimeMark && first.TimeMark > new TimeSpan(0, 0, 3))
                    {
                        RemainToStart = _currentPos / first.TimeMark * 100;
                        RemainToStartBarVisibility = Visibility.Visible;
                    }

                    var c = _lyrics.Where(l => l.TimeMark.Minutes == value.Minutes && l.TimeMark.Seconds == value.Seconds && l.TimeMark.Milliseconds - value.Milliseconds <= 5).FirstOrDefault();
                    if (c == null || c == new Lyrics()) return;

                    if (Current != null) Current.IsCurrent = false;
                    Current = null;
                    Current = c;
                    c.IsCurrent = true;
                }
                else if (value > new TimeSpan(0, 0, 1))
                {
                    NoLyrics = true;
                    MiniLyricsPanelVisibility = Visibility.Collapsed;
                }
            }
        }

        [ObservableProperty]
        private Lyrics _current;
        [ObservableProperty]
        private Visibility _lyricsPanelVisibility = Visibility.Collapsed;
        [ObservableProperty]
        private Visibility _miniLyricsPanelVisibility = Visibility.Collapsed;
        [ObservableProperty]
        private Visibility _remainToStartBarVisibility = Visibility.Collapsed;
        [ObservableProperty]
        private double _remainToStart;
        [ObservableProperty]
        private bool _noLyrics
            = true;

        public ScrollViewer LyricsScroller { get; set; }
    }
}

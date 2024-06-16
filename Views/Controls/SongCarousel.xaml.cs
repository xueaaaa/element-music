using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.Json;
using System.Windows.Controls;
using ElementMusic.Models.ElementAPI;

namespace ElementMusic.Views.Controls
{
    public enum SongType
    {
        Latest,
        Random,
        Favorites
    }

    public partial class SongCarousel : Grid
    {
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(ObservableCollection<Song>), typeof(SongCarousel));
        public static readonly DependencyProperty SongTypeProperty =
            DependencyProperty.Register("SongType", typeof(SongType), typeof(SongCarousel));


        public SongType SongType
        {
            get => (SongType)GetValue(SongTypeProperty);
            set => SetValue(SongTypeProperty, value);
        }

        public ObservableCollection<Song> Source
        {
            get => (ObservableCollection<Song>)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public SongCarousel()
        {
            InitializeComponent();
            LoadSongs();
        }

        public async void LoadSongs(bool startsFromLast = false)
        {
            string type = string.Empty;
            switch ((SongType)Tag)
            {
                case SongType.Latest:
                    type = "LATEST";
                    break;
                case SongType.Random:
                    type = "RANDOM";
                    break;
                case SongType.Favorites:
                    type = "FAVORITES";
                    break;
                default:
                    type = "LATEST";
                    break;
            }

            HttpResponseMessage? obj = await App.APISender.SendRequest($"LoadSongs.php?F={type}", HttpMethod.Post, new Dictionary<string, object>()
            {
                { "StartIndex", startsFromLast ? Source.Count : 0 }
            });
            ObservableCollection<Song>? songs = JsonSerializer.Deserialize<ObservableCollection<Song>?>(await obj.Content.ReadAsStringAsync());

            if (Source != null)
                foreach (var item in songs)
                    Source.Add(item);
            else Source = songs;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var scrollViewer = GetScrollViewer();
            scrollViewer?.LineLeft();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var scrollViewer = GetScrollViewer();
            scrollViewer?.LineRight();
        }

        private ScrollViewer? GetScrollViewer()
        {
            var template = SongsControl.Template;
            return template.FindName("SongsScrollViewer", SongsControl) as ScrollViewer;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e) =>
            LoadSongs(true);
    }
}

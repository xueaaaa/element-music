using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.Json;
using System.Windows.Controls;
using ElementMusic.Models.ElementAPI;
using ElementMusic.ViewModels.Windows;

namespace ElementMusic.Views.Controls
{
    public enum SongType
    {
        None,
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
        public static readonly DependencyProperty IsSourceLoadingProperty =
            DependencyProperty.Register("IsSourceLoading", typeof(bool), typeof(SongCarousel));

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

        public bool IsSourceLoading
        {
            get => (bool)GetValue(IsSourceLoadingProperty);
            private set => SetValue(IsSourceLoadingProperty, value);
        }

        public SongCarousel()
        {
            InitializeComponent();
            
            Loaded += (_, _) =>
            {
                if(Source == null || Source.Count == 0)
                    LoadSongs();
            };
        }

        public async void LoadSongs(bool startsFromLast = false)
        {
            IsSourceLoading = true;

            string f = string.Empty;
            switch (SongType)
            {
                case SongType.None:
                    IsSourceLoading = false;
                    return;
                case SongType.Latest:
                    f = "LATEST";
                    break;
                case SongType.Random:
                    f = "RANDOM";
                    break;
                case SongType.Favorites:
                    f = "FAVORITES";
                    break;
                default:
                    f = "LATEST";
                    break;
            }

            HttpResponseMessage? obj = await App.ElementAPISender.SendRequest($"LoadSongs.php?F={f}", HttpMethod.Post, new Dictionary<string, object>()
            {
                { "StartIndex", startsFromLast ? Source.Count : 0 }
            });
            ObservableCollection<Song>? songs = JsonSerializer.Deserialize<ObservableCollection<Song>?>(await obj.Content.ReadAsStringAsync());
            foreach (var item in songs)
                if (item.Cover == null)
                    item.Cover = new SongImages
                    {
                        SimpleImage = "/resources/images/empty_cover.jpg"
                    }; 

            if (Source != null && startsFromLast)
                foreach (var item in songs)
                    Source.Add(item);
            else Source = songs;

            IsSourceLoading = false;
        }

        private void ScrollLeftButtonClick(object sender, RoutedEventArgs e)
        {
            var scrollViewer = GetScrollViewer();
            scrollViewer?.LineLeft();
        }

        private void ScrollRightButtonClick(object sender, RoutedEventArgs e)
        {
            var scrollViewer = GetScrollViewer();
            scrollViewer?.LineRight();
        }

        private ScrollViewer? GetScrollViewer()
        {
            var template = SongsControl.Template;
            return template.FindName("SongsScrollViewer", SongsControl) as ScrollViewer;
        }

        private void LoadMoreButtonClick(object sender, RoutedEventArgs e) =>
            LoadSongs(true);

        private void AddAllToQueueButtonClick(object sender, RoutedEventArgs e) 
        {
            var player = App.GetService<MainWindowViewModel>().SongPlayerViewModel;
            foreach (var item in Source)
                player.AddToQueue(item);
        }

        private void ReloadButtonClick(object sender, RoutedEventArgs e) =>
            LoadSongs();
    }
}

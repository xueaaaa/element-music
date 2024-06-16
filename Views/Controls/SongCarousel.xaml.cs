using System.Windows.Controls;
using ElementMusic.ViewModels.Controls;

namespace ElementMusic.Views.Controls
{
    public enum SongsType
    {
        Latest,
        Random,
        Favorites
    }

    public partial class SongCarousel : Grid
    {
        public static readonly DependencyProperty SongsTypeProperty =
            DependencyProperty.Register("SongsType", typeof(SongsType), typeof(SongCarousel));

        public SongsType SongsType
        {
            get => (SongsType)GetValue(SongsTypeProperty);
            set => SetValue(SongsTypeProperty, value);
        }

        public SongCarouselViewModel ViewModel { get; set; }

        public SongCarousel()
        {
            ViewModel = new SongCarouselViewModel(SongsType);
            ViewModel.LoadSongs();

            InitializeComponent();
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
            ViewModel.LoadSongs(true);
    }
}

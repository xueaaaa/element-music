using ElementMusic.Models.ElementAPI;
using ElementMusic.ViewModels.Helpers;
using ElementMusic.ViewModels.Windows;
using System.Windows.Controls;

namespace ElementMusic.Views.Controls
{
    public partial class SongCard : Grid
    {
        public static readonly DependencyProperty SongProperty =
            DependencyProperty.Register("Song", typeof(Song), typeof(SongCard));

        public Song Song
        {
            get => (Song)GetValue(SongProperty);
            set => SetValue(SongProperty, value);
        }

        public FlyoutViewModel FlyoutViewModel { get; set; }
            = new FlyoutViewModel();

        public SongCard()
        {
            InitializeComponent();
        }

        private void Grid_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var player = App.GetService<MainWindowViewModel>().SongPlayerViewModel;
            player.StartFromNew(Song);
        }
    }
}

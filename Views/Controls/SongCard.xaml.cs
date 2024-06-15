using ElementMusic.Models.ElementAPI;
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

        public SongCard()
        {
            InitializeComponent();
        }
    }
}

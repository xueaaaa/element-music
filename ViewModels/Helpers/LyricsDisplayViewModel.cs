using ElementMusic.Models.MusixmatchAPI;

namespace ElementMusic.ViewModels.Helpers
{
    public partial class LyricsDisplayViewModel : ObservableObject
    {
        [ObservableProperty]
        private List<Lyrics> _lyrics;
    }
}

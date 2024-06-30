using System.Collections.ObjectModel;

namespace ElementMusic.Models.ElementAPI
{
    public partial class SongSearchResponse : ObservableObject
    {
        [ObservableProperty]
        private string _category;
        [ObservableProperty]
        private ObservableCollection<Song> _content;
    }
}

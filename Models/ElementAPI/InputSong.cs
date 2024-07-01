
namespace ElementMusic.Models.ElementAPI
{
    public partial class InputSong : ObservableObject
    {
        [ObservableProperty]
        private string _title;
        [ObservableProperty] 
        private string _artist;
        [ObservableProperty]
        private string _album;
        [ObservableProperty]
        private string _genre;
        [ObservableProperty]
        private int _releaseYear = 2000;
        [ObservableProperty]
        private string _composer;
        [ObservableProperty]
        private string _pubText;
        [ObservableProperty]
        private string _fileName;
        [ObservableProperty]
        private string _imageName;

        [ObservableProperty]
        private string _file;
        [ObservableProperty]
        private string _image;
    }
}

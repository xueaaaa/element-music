using System.Text.Json.Serialization;

namespace ElementMusic.Models.ElementAPI
{
    public partial class Song : ObservableObject
    {
        [ObservableProperty]
        private string? _title;
        [ObservableProperty]
        private string? _artist;
        [ObservableProperty]
        private string? _album;
        [ObservableProperty]
        private SongImages? _cover;
        [ObservableProperty]
        private string? _file;
        [ObservableProperty]
        private string? _genre;
        [ObservableProperty]
        private string? _releaseYear;
        [ObservableProperty]
        private string? _composer;
        [ObservableProperty]
        private string? _duration;
        [ObservableProperty]
        private int? _bitrate;
        [ObservableProperty]
        private string? _audioFormat;
        [ObservableProperty]
        private string? _dateAdded;
        [ObservableProperty]
        private int? _order;
        [ObservableProperty]
        private bool? _liked;
    }

    public partial class SongImages : ObservableObject
    {
        [ObservableProperty]
        private string? _image;

        private string _simpleImage;
        [JsonPropertyName("simple_image")]
        public string SimpleImage
        {
            get => _simpleImage;
            set
            {
                _simpleImage = value;
                OnPropertyChanged(nameof(SimpleImage));
            }
        }

    }
}

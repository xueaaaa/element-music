using ElementMusic.ViewModels.Windows;
using System.Net.Http;
using System.Text.Json.Serialization;

namespace ElementMusic.Models.ElementAPI
{
    public partial class Song : ObservableObject
    {
        [ObservableProperty]
        private int? _ID;
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

        [RelayCommand]
        private void Play() =>
            App.GetService<MainWindowViewModel>().SongPlayerViewModel.StartFromNew(this);

        [RelayCommand]
        private async void SetLike()
        {
            await App.ElementAPISender.SendRequest("MusicInteraction.php?F=LIKE", HttpMethod.Post, new Dictionary<string, object>
            {
                { "SongID", ID}
            });
            Liked = !Liked;
        }

        [RelayCommand]
        private void ToQueue() =>
            App.GetService<MainWindowViewModel>().SongPlayerViewModel.AddToQueue(this);
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

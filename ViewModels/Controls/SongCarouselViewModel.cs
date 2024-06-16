using ElementMusic.Models.ElementAPI;
using ElementMusic.Views.Controls;
using System.Net.Http;
using System.Text.Json;

namespace ElementMusic.ViewModels.Controls
{
    public partial class SongCarouselViewModel : ObservableObject
    {
        [ObservableProperty]
        private List<Song>? _source;
        [ObservableProperty]
        private SongsType _songsType;

        public SongCarouselViewModel(SongsType songsType) =>
            _songsType = songsType;

        public async void LoadSongs(bool startsFromLast = false)
        {
            string type = string.Empty;
            switch (SongsType)
            {
                case SongsType.Latest:
                    type = "LATEST";
                    break;
                case SongsType.Random:
                    type = "RANDOM";
                    break;
                case SongsType.Favorites:
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
            List<Song>? songs = JsonSerializer.Deserialize<List<Song>?>(await obj.Content.ReadAsStringAsync());

            var list = new List<Song>();
            if(Source != null) list.AddRange(Source);
            list.AddRange(songs);
            Source = list;
        }
    }
}

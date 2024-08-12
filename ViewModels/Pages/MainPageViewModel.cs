using ElementMusic.Models.ElementAPI;
using ElementMusic.ViewModels.Helpers;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.Json;
using Wpf.Ui.Controls;

namespace ElementMusic.ViewModels.Pages
{
    public partial class MainPageViewModel : ObservableObject, INavigationAware
    {
        [ObservableProperty]
        private InfoBarViewModel _infoBarViewModel;

        [ObservableProperty]
        private string _searchText
            = string.Empty;
        [ObservableProperty]
        private ObservableCollection<Song> _searchedSongs
            = new();
        [ObservableProperty]
        private Visibility _searchedCarouselVisibility
            = Visibility.Collapsed;

        public void OnNavigatedFrom() { }

        public async void OnNavigatedTo()
        {

        }

        [RelayCommand]
        private async void Search()
        {
            if (SearchText == string.Empty)
            {
                SearchedSongs.Clear();
                SearchedCarouselVisibility = Visibility.Collapsed;
                return;
            }

            var resp = await App.ElementAPISender.SendRequest("Search.php", HttpMethod.Post, new Dictionary<string, object>
            {
                { "SearchVal", SearchText.Trim() },
                { "Category", "Music" }
            });

            var parsed = JsonSerializer.Deserialize<SongSearchResponse>(await resp.Content.ReadAsStringAsync());
            
            if(parsed.Content.Count > 0)
            {
                var songs = new ObservableCollection<Song>();   
                foreach (var item in parsed.Content)
                {
                    var songRaw = await App.ElementAPISender.SendRequest("LoadSong.php", HttpMethod.Post, new Dictionary<string, object>
                    {
                        { "SongID", item.ID }
                    });
                    var s = await songRaw.Content.ReadAsStringAsync();
                    var song = JsonSerializer.Deserialize<Song>(s);
                    songs.Add(song);
                }

                SearchedSongs = songs;
                SearchedCarouselVisibility = Visibility.Visible;
            }
            else
            {
                SearchedSongs.Clear();
            }
        }
    }
}

using ElementMusic.Models.ElementAPI;
using ElementMusic.ViewModels.Helpers;
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
        private List<Song>? _songList;

        public void OnNavigatedFrom() { }

        public async void OnNavigatedTo()
        {
            try
            {
                HttpResponseMessage? obj = await App.APISender.SendRequest("LoadSongs.php?F=LATEST", HttpMethod.Post);
                List<Song>? songs = JsonSerializer.Deserialize<List<Song>?>(await obj.Content.ReadAsStringAsync());
                SongList = songs;
            }
            catch (Exception ex)
            {
                InfoBarViewModel.ErrorTemplate(ex.Message);
            }
        }
    }
}

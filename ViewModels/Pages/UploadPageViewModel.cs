using ElementMusic.Models.ElementAPI;
using ElementMusic.ViewModels.Helpers;
using Microsoft.Win32;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using Wpf.Ui.Controls;

namespace ElementMusic.ViewModels.Pages
{
    public partial class UploadPageViewModel : ObservableObject, INavigationAware
    {
        [ObservableProperty]
        private InputSong _input
            = new();
        [ObservableProperty]
        private InfoBarViewModel _infoBarViewModel
            = new();

        public void OnNavigatedFrom() { }

        public void OnNavigatedTo() { }

        [RelayCommand]
        private void OpenFile()
        {
            OpenFileDialog ofd = new();
            ofd.Filter = "MPEG-1 Audio Layer III (*.mp3)|*.mp3|" +
                "Waveform Audio File Format (Element Gold required) (*.wav)|*.wav|" +
                "Free Lossless Audio Codec (Element Gold required) (*.flac)|*.flac";
            if(ofd.ShowDialog() == true)
            {
                Input.FileName = Path.GetFileName(ofd.FileName);
                Input.File = ofd.FileName;
            }
        }

        [RelayCommand]
        private void OpenCover()
        {
            OpenFileDialog ofd = new();
            ofd.Filter = "Portable Network Graphics (*.png)|*.png|" +
                "Joint Photographic Experts Group (*.jpg;*.jpeg)|*.jpg;*.jpeg";

            if (ofd.ShowDialog() == true)
            {
                Input.ImageName = Path.GetFileName(ofd.FileName);
                Input.Image = ofd.FileName;
            }
        }

        [RelayCommand]
        private async void Send()
        {
            var resp = await App.APISender.SendRequest("AddMusic.php", HttpMethod.Post, new Dictionary<string, object>
            {
                { "Title", Input.Title },
                { "Artist", Input.Artist },
                { "Album", Input.Album },
                { "Genre", Input.Genre },
                { "ReleaseYear", Convert.ToString(Input.ReleaseYear) },
                { "Composer", Input.Composer },
                { "PostText", Input.PubText },
                { "AudioFile", Input.File },
                { "CoverFile", Input.Image }
            });

            var servResp = JsonSerializer.Deserialize<ServerResponse>(await resp.Content.ReadAsStringAsync());
            if (servResp?.Type == ServerResponse.ERROR_TYPE)
                InfoBarViewModel.ErrorTemplate(servResp.Content);
            else if (servResp?.Type == ServerResponse.CONTENT_TYPE)
                InfoBarViewModel.SuccessTemplate();
            else InfoBarViewModel.ErrorTemplate();
        }
    }
}

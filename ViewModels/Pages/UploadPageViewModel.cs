using ElementMusic.Models.ElementAPI;
using Microsoft.Win32;
using System.IO;
using Wpf.Ui.Controls;

namespace ElementMusic.ViewModels.Pages
{
    public partial class UploadPageViewModel : ObservableObject, INavigationAware
    {
        [ObservableProperty]
        private InputSong _input;

        public void OnNavigatedFrom() { }

        public void OnNavigatedTo() { }

        [RelayCommand]
        private async void OpenFile()
        {
            OpenFileDialog ofd = new();
            ofd.Filter = "MPEG-1 Audio Layer III (*.mp3)|*.mp3|" +
                "Waveform Audio File Format (*.wav)|*.wav|" +
                "Audio Interchange File Format (*.aiff)|*.aiff|" +
                "Free Lossless Audio Codec (*.flac)|*.flac|" +
                "Advanced Audio Coding (*.aac)|*.aac";

            if(ofd.ShowDialog() == true)
            {
                Input.FileName = Path.GetFileName(ofd.FileName);
                Input.File = await File.ReadAllBytesAsync(Input.FileName);
            }
        }

        [RelayCommand]
        private async void OpenCover()
        {
            OpenFileDialog ofd = new();
            ofd.Filter = "Portable Network Graphics (*.png)|*.png|" +
                "Joint Photographic Experts Group (*.jpg;*.jpeg)|*.jpg;*.jpeg|";

            if (ofd.ShowDialog() == true)
            {
                Input.ImageName = Path.GetFileName(ofd.FileName);
                Input.Image = await File.ReadAllBytesAsync(Input.FileName);
            }
        }
    }
}

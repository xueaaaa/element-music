using ElementMusic.ViewModels.Windows;

namespace ElementMusic.Models.MusixmatchAPI
{
    public partial class Lyrics : ObservableObject
    {
        public TimeSpan TimeMark { get; set; }
        public string Content { get; set; }

        [ObservableProperty]
        private bool _isCurrent;

        [RelayCommand]
        private void MoveToTime() =>
            App.GetService<MainWindowViewModel>().SongPlayerViewModel.MoveToTime(TimeMark);
    }
}

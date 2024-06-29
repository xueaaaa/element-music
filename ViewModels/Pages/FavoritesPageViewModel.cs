#pragma warning disable CS8601
#pragma warning disable CS8602

using ElementMusic.Models.ElementAPI;
using ElementMusic.ViewModels.Helpers;
using ElementMusic.Views.Pages;
using System.Collections.ObjectModel;
using Wpf.Ui.Controls;

namespace ElementMusic.ViewModels.Pages
{
    public partial class FavoritesPageViewModel : ObservableObject, INavigationAware
    {
        [ObservableProperty]
        private InfoBarViewModel _infoBarViewModel;

        private bool _onceSearched;
        private ObservableCollection<Song> _orig;

        public void OnNavigatedFrom() { }

        public void OnNavigatedTo() { }

        [RelayCommand]
        private void Search(object param)
        {
            var page = App.GetService<FavoritesPage>();

            if (!_onceSearched)
                _orig = page.SongCarousel.Source;

            if (param.ToString() == string.Empty)
            {
                page.SongCarousel.Source = _orig;
                _onceSearched = false;
                return;
            }

            var ne = new ObservableCollection<Song>(page.SongCarousel.Source.Where(s =>
            s.Title.ToLower().StartsWith(param.ToString().ToLower())));
            page.SongCarousel.Source = ne;
            _onceSearched = true;
        }
    }
}

#pragma warning restore CS8602
#pragma warning restore CS8601
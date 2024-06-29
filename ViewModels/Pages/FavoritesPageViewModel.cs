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

        public void OnNavigatedFrom() { }

        public void OnNavigatedTo() { }

        [RelayCommand]
        private void Search(object param)
        {
            var page = App.GetService<FavoritesPage>();

            if (param.ToString() == string.Empty) return;

            page.SongCarousel.Source = page.SongCarousel.Source.Where(s => 
            s.Title.StartsWith(param.ToString()) || s.Title.EndsWith(param.ToString())) 
                as ObservableCollection<Song>;
        }
    }
}

#pragma warning restore CS8602
#pragma warning restore CS8601
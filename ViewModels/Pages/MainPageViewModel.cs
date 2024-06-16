using ElementMusic.Models.ElementAPI;
using ElementMusic.ViewModels.Helpers;
using Wpf.Ui.Controls;

namespace ElementMusic.ViewModels.Pages
{
    public partial class MainPageViewModel : ObservableObject, INavigationAware
    {
        [ObservableProperty]
        private InfoBarViewModel _infoBarViewModel;

        public void OnNavigatedFrom() { }

        public async void OnNavigatedTo()
        {

        }
    }
}

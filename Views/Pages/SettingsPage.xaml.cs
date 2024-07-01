using ElementMusic.ViewModels.Pages;
using Wpf.Ui.Controls;

namespace ElementMusic.Views.Pages
{
    public partial class SettingsPage : INavigableView<SettingsViewModel>
    {
        public SettingsViewModel ViewModel { get; }

        public SettingsPage(SettingsViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) =>
            ViewModel.ChangeVolumeCommand.Execute(null);
    }
}

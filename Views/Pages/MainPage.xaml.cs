using ElementMusic.ViewModels.Pages;
using Wpf.Ui.Controls;

namespace ElementMusic.Views.Pages
{
    public partial class MainPage : INavigableView<MainPageViewModel>
    {
        public MainPageViewModel ViewModel { get; }

        public MainPage(MainPageViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            ViewModel.SearchCommand.Execute(((TextBox)sender).Text);
        }
    }
}

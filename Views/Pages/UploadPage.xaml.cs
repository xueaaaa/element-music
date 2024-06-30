using ElementMusic.ViewModels.Pages;
using Wpf.Ui.Controls;

namespace ElementMusic.Views.Pages
{
    public partial class UploadPage : INavigableView<UploadPageViewModel>
    {
        public UploadPageViewModel ViewModel { get; }

        public UploadPage(UploadPageViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }
    }
}

namespace ElementMusic.ViewModels.Windows
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool _isAuthorized;

        public MainWindowViewModel()
        {
            IsAuthorized = Properties.Settings.Default.AuthorizationToken == string.Empty ?
                false : true;
        }
    }
}

namespace ElementMusic.ViewModels.Helpers
{
    public partial class FlyoutViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool _isOpened;

        [RelayCommand]
        private void Open()
        {
            if (!IsOpened)
                IsOpened = true;
        }
    }
}

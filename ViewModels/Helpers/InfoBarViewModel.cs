using Wpf.Ui.Controls;

namespace ElementMusic.ViewModels.Helpers
{
    public partial class InfoBarViewModel : ObservableObject
    {
        private System.Timers.Timer _timeoutTimer;

        [ObservableProperty]
        private string _title;

        [ObservableProperty]
        private string _message;

        [ObservableProperty]
        private bool _isOpen;

        [ObservableProperty]
        private InfoBarSeverity _severity;

        public InfoBarViewModel()
        {
            _timeoutTimer = new System.Timers.Timer(5000);
            _timeoutTimer.Elapsed += (_, _) => Hide();
        }

        public void SetNew(string title = "", string message = "", InfoBarSeverity severity = InfoBarSeverity.Informational)
        {
            this.Title = title;
            this.Message = message;
            this.Severity = severity;
            this.IsOpen = true;

            _timeoutTimer.Start();
        }

        public void Hide()
        {
            IsOpen = false;
            _timeoutTimer.Stop();
        }

        public void SuccessTemplate(string message = "") => SetNew(
            Application.Current.Resources["InfoBarSuccessTitle"].ToString(), 
            message, 
            InfoBarSeverity.Success);

        public void WarningTemplate(string message = "") => SetNew(
            Application.Current.Resources["InfoBarWarningTitle"].ToString(),
            message,
            InfoBarSeverity.Warning);

        public void ErrorTemplate(string message = "") => SetNew(
            Application.Current.Resources["InfoBarErrorTitle"].ToString(),
            message, 
            InfoBarSeverity.Error);
    }
}

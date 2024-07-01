using ElementMusic.Models.ElementAPI;
using ElementMusic.ViewModels.Helpers;
using ElementMusic.ViewModels.Pages;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Text.Json;

namespace ElementMusic.ViewModels.Windows
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private InfoBarViewModel _infoBarViewModel = new InfoBarViewModel();
        [ObservableProperty]
        private SongPlayerViewModel _songPlayerViewModel = new SongPlayerViewModel();
        [ObservableProperty]
        private  SettingsViewModel _settingsViewModel;
        [ObservableProperty]
        private bool _isAuthorized;

        [Required]
        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        [Required]
        [MinLength(4)]
        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        [ObservableProperty]
        private bool _saveData;

        public MainWindowViewModel()
        {
            if (Properties.Settings.Default.LastAuthorizationDay.AddDays(3) < DateTime.Now)
                IsAuthorized = false;
            else if(Properties.Settings.Default.SessionKey == string.Empty) 
                IsAuthorized = false;
            else IsAuthorized = true;

            Email = Properties.Settings.Default.Email;
            Password = Properties.Settings.Default.Password;

            if (!IsAuthorized && Properties.Settings.Default.Email != string.Empty)
                Login(null);

            SaveData = true;

            SettingsViewModel = App.GetService<SettingsViewModel>();

            Properties.Settings.Default.PropertyChanged += (o, e) =>
            {
                if (e.PropertyName == nameof(Properties.Settings.Default.SessionKey))
                    IsAuthorized = Properties.Settings.Default.SessionKey == string.Empty ?
                        false : true;
            };
        }

        [RelayCommand]
        private async void Login(object param)
        {
            var results = new List<ValidationResult>();

            if (!Validator.TryValidateValue(
                _email,
                new ValidationContext(_email),
                results,
                new List<ValidationAttribute>
                {
                    new EmailAddressAttribute()
                }))
            {
                InfoBarViewModel.ErrorTemplate($"{Application.Current.Resources["IncorrectEmailError"]}: \n{results[0].ErrorMessage}");
                return;
            }

            HttpResponseMessage? msg = await App.APISender.SendRequest("Authorization.php?F=LOGIN", HttpMethod.Post, new Dictionary<string, object>
            {
                { "Email", Email.Trim() },
                { "Password", Password.Trim() }
            });

            ServerResponse? resp = JsonSerializer.Deserialize<ServerResponse>(await msg.Content.ReadAsStringAsync());

            if (resp.Type == ServerResponse.ERROR_TYPE)
                InfoBarViewModel.ErrorTemplate(resp.Content);
            else
            {
                Properties.Settings.Default.SessionKey = resp.Content;
                Properties.Settings.Default.Email = SaveData ? Email : string.Empty;
                Properties.Settings.Default.Password = SaveData ? Password : string.Empty;
                Properties.Settings.Default.LastAuthorizationDay = DateTime.Now;    
            }
        }
    }
}

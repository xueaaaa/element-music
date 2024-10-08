﻿using ElementMusic.Models;
using ElementMusic.ViewModels.Helpers;
using ElementMusic.ViewModels.Windows;
using System.Collections.ObjectModel;
using System.Globalization;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;

namespace ElementMusic.ViewModels.Pages
{
    public partial class SettingsViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;

        [ObservableProperty]
        private string _appVersion = string.Empty;

        [ObservableProperty]
        private ApplicationTheme _currentTheme = ApplicationTheme.Unknown;

        private ObservableCollection<SettingsParameterViewModel> _languages;
        public ObservableCollection<SettingsParameterViewModel> Languages
        {
            get
            {
                if (_languages == null)
                {
                    var items = new ObservableCollection<SettingsParameterViewModel>();
                    foreach (var lang in Localizator.AppLanguages)
                    {
                        var item = new SettingsParameterViewModel
                        {
                            DisplayName = char.ToUpper(lang.NativeName[0]) + lang.NativeName.Substring(1),
                            Tag = lang.Name
                        };

                        items.Add(item);
                    }

                    _languages = items;
                }

                return _languages;
            }
        }

        private SettingsParameterViewModel _selectedLanguage;
        public SettingsParameterViewModel SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                if(_selectedLanguage == null)
                {
                    _selectedLanguage = value;
                    OnPropertyChanged(nameof(SelectedLanguage));
                }

                else if (_selectedLanguage != value && new CultureInfo(_selectedLanguage.Tag.ToString()) != Properties.Settings.Default.Language)
                {
                    _selectedLanguage = value;
                    var info = new CultureInfo(_selectedLanguage.Tag.ToString());
                    Localizator.CurrentLanguage = info;
                    OnPropertyChanged(nameof(SelectedLanguage));
                }
            }
        }

        private ObservableCollection<SettingsParameterViewModel> _visibilities;
        public ObservableCollection<SettingsParameterViewModel> Visibilities
        {
            get
            {
                if (_visibilities == null)
                {
                    var items = new ObservableCollection<SettingsParameterViewModel>()
                    {
                        new() {
                            DisplayName = $"{Application.Current.Resources["VisibleName"]}",
                            Tag = Visibility.Visible
                        },

                        new() {
                            DisplayName = $"{Application.Current.Resources["CollapsedName"]}",
                            Tag = Visibility.Collapsed
                        }
                    };

                    _visibilities = items;
                }

                return _visibilities;
            }
        }

        private SettingsParameterViewModel _miniLyricsDisplayVisibility;
        public SettingsParameterViewModel MiniLyricsDisplayVisibility
        {
            get => _miniLyricsDisplayVisibility;
            set
            {
                _miniLyricsDisplayVisibility = value;
                App.GetService<MainWindowViewModel>().SongPlayerViewModel.LyricsDisplayViewModel.MiniLyricsPanelVisibility = (Visibility)value.Tag;
                OnPropertyChanged(nameof(MiniLyricsDisplayVisibility));
            }
        }

        [ObservableProperty]
        private int _volume;

        public void OnNavigatedTo()
        {
            if (!_isInitialized)
                InitializeViewModel();
        }

        public void OnNavigatedFrom() { }

        private void InitializeViewModel()
        {
            CurrentTheme = ApplicationThemeManager.GetAppTheme();
            SelectedLanguage = Languages.Where(l => l.Tag.ToString() == Properties.Settings.Default.Language.Name).First();
            AppVersion = GetAssemblyVersion();
            Volume = Properties.Settings.Default.Volume;

            var savedMLDVisibility = Properties.Settings.Default.MiniLyricsDisplayVisibility;
            MiniLyricsDisplayVisibility = Visibilities.Where(v => (Visibility)v.Tag == savedMLDVisibility).FirstOrDefault();

            _isInitialized = true;
        }

        private static string GetAssemblyVersion() =>
            System.Reflection.Assembly.GetExecutingAssembly().GetName().Version?.ToString()
                ?? string.Empty;

        [RelayCommand]
        private void OnChangeTheme(string parameter)
        {
            switch (parameter)
            {
                case "theme_light":
                    if (CurrentTheme == ApplicationTheme.Light)
                        break;

                    ApplicationThemeManager.Apply(ApplicationTheme.Light);
                    CurrentTheme = ApplicationTheme.Light;

                    break;

                default:
                    if (CurrentTheme == ApplicationTheme.Dark)
                        break;

                    ApplicationThemeManager.Apply(ApplicationTheme.Dark);
                    CurrentTheme = ApplicationTheme.Dark;

                    break;
            }
        }

        [RelayCommand]
        private void ChangeVolume()
        {
            var player = App.GetService<MainWindowViewModel>().SongPlayerViewModel;
            player.Volume = Volume;
            player.VolumeChanged(true);
        }

        [RelayCommand]
        private void Logout(object param) => Properties.Settings.Default.SessionKey = string.Empty;
    }
}

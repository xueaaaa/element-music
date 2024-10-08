﻿using ElementMusic.Models;
using ElementMusic.Models.ElementAPI;
using ElementMusic.Properties;
using ElementMusic.Services;
using ElementMusic.ViewModels.Pages;
using ElementMusic.ViewModels.Windows;
using ElementMusic.Views.Pages;
using ElementMusic.Views.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Threading;
using Wpf.Ui;
using VersionControl;
using Yandex.Metrica;
using ElementMusic.Models.MusixmatchAPI;

namespace ElementMusic
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private static readonly IHost _host = Host
            .CreateDefaultBuilder()
            .ConfigureAppConfiguration(c => { c.SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)); })
            .ConfigureServices((context, services) =>
            {
                services.AddHostedService<ApplicationHostService>();

                // Page resolver service
                services.AddSingleton<IPageService, PageService>();

                // Theme manipulation
                services.AddSingleton<IThemeService, ThemeService>();

                // TaskBar manipulation
                services.AddSingleton<ITaskBarService, TaskBarService>();

                // Service containing navigation, same as INavigationWindow... but without window
                services.AddSingleton<INavigationService, NavigationService>();

                // Main window with navigation
                services.AddSingleton<INavigationWindow, MainWindow>();
                services.AddSingleton<MainWindowViewModel>();

                services.AddSingleton<MainPage>();
                services.AddSingleton<MainPageViewModel>();
                services.AddSingleton<FavoritesPage>();
                services.AddSingleton<FavoritesPageViewModel>();
                services.AddSingleton<UploadPage>();
                services.AddSingleton<UploadPageViewModel>();
                services.AddSingleton<SettingsPage>();
                services.AddSingleton<SettingsViewModel>();
            }).Build();

        internal static readonly ElementAPISender ElementAPISender 
            = new();
        internal static readonly MusixmatchAPISender MusixmatchAPISender 
            = new();

        /// <summary>
        /// Gets registered service.
        /// </summary>
        /// <typeparam name="T">Type of the service to get.</typeparam>
        /// <returns>Instance of the service or <see langword="null"/>.</returns>
        public static T GetService<T>()
            where T : class
        {
            return _host.Services.GetService(typeof(T)) as T;
        }

        /// <summary>
        /// Occurs when the application is loading.
        /// </summary>
        private void OnStartup(object sender, StartupEventArgs e)
        {
            Localizator.AddLang(new CultureInfo("en"));
            Localizator.AddLang(new CultureInfo("ru"));

            Localizator.LanguageChanged += (_, _) =>
            {
                Settings.Default.Language = Localizator.CurrentLanguage;
                Settings.Default.Save();
            };

            var lang = Settings.Default.Language;
            Localizator.CurrentLanguage = lang == null ?
                Settings.Default.DefaultLanguage :
                lang;

            Settings.Default.PropertyChanged += (_, _) => Settings.Default.Save();

            Parameters.Set("element-music", "xueaaaa", "Update.zip", $"{Directory.GetCurrentDirectory()}\\Update.zip");
            YandexMetricaFolder.SetCurrent("AppMetrica");
            YandexMetrica.Activate("805bdafa-36f3-4a9c-94c2-e914014f7ac9");

            _host.Start();
        }

        /// <summary>
        /// Occurs when the application is closing.
        /// </summary>
        private async void OnExit(object sender, ExitEventArgs e)
        {
            await _host.StopAsync();

            _host.Dispose();
        }

        /// <summary>
        /// Occurs when an exception is thrown by an application but not handled.
        /// </summary>
        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e) => e.Handled = true;
    }
}

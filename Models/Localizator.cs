using System.Globalization;

namespace ElementMusic.Models
{
    /// <summary>
    /// Class for working with application locales
    /// </summary>
    public static class Localizator
    {
        /// <summary>
        /// Called when the application language is changed
        /// </summary>
        public static event EventHandler LanguageChanged;

        private static List<CultureInfo> _appLanguages = new List<CultureInfo>();
        /// <summary>
        /// List of all available application languages in CultureInfo format 
        /// </summary>
        public static List<CultureInfo> AppLanguages
        {
            get { return _appLanguages; }
        }

        /// <summary>
        /// Current application language
        /// </summary>
        public static CultureInfo CurrentLanguage
        {
            get => Thread.CurrentThread.CurrentUICulture;
            set
            {
                if (value == null) throw new ArgumentNullException("cannot use null language");
                if (value == Thread.CurrentThread.CurrentUICulture) return;

                Thread.CurrentThread.CurrentUICulture = value;
                ResourceDictionary dictionary = new ResourceDictionary();

                if (value.TwoLetterISOLanguageName == Properties.Settings.Default.DefaultLanguage.TwoLetterISOLanguageName)
                    dictionary.Source = new Uri("Resources/Locales/Locale.xaml", UriKind.Relative);
                else
                    dictionary.Source = new Uri($"Resources/Locales/{value.Name}.Locale.xaml", UriKind.Relative);

                // Search for the dictionary of the previous locale
                ResourceDictionary oldDictionary = Application.Current.Resources.MergedDictionaries.Where(d =>
                d.Source != null && d.Source.OriginalString.EndsWith("Locale.xaml")).First();

                if (oldDictionary != null)
                {
                    // Replacing the dictionary of the previous locale with a new one
                    int i = Application.Current.Resources.MergedDictionaries.IndexOf(oldDictionary);
                    Application.Current.Resources.MergedDictionaries.Remove(oldDictionary);
                    Application.Current.Resources.MergedDictionaries.Insert(i, dictionary);
                }
                else
                {
                    Application.Current.Resources.MergedDictionaries.Add(dictionary);
                }

                LanguageChanged(Application.Current, new EventArgs());
            }
        }

        /// <summary>
        /// Adds a new language to the list of available application languages
        /// </summary>
        /// <param name="culture">Language</param>
        public static void AddLang(CultureInfo culture)
        {
            _appLanguages.Add(culture);
        }
    }
}

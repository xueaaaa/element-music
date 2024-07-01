using System.Globalization;
using System.Windows.Data;

namespace ElementMusic.Helpers
{
    public class UriConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string emptyCoverPath = "/resources/images/empty_cover.jpg";

            if (value?.ToString() == emptyCoverPath) return emptyCoverPath;

            if (parameter.ToString() == "CoverUri")
                return $"https://elemsocial.com/Content/Music/Covers/{value}";
            else if (parameter.ToString() == "SimpleCoverUri")
                return $"https://elemsocial.com/Content/Simple/{value}";

            return emptyCoverPath;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

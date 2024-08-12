using System.Globalization;
using System.Windows.Data;

namespace ElementMusic.Helpers
{
    public class ListCountToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is int i) return i > 0 ? Visibility.Collapsed : Visibility.Visible;
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

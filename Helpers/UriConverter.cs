﻿using System.Globalization;
using System.Windows.Data;

namespace ElementMusic.Helpers
{
    public class UriConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter.ToString() == "CoverUri")
                return value == string.Empty ? "Resources/Images/empty_cover.jpg" : $"https://elemsocial.com/Content/Music/Covers/{value}";
            else if (parameter.ToString() == "SimpleCoverUri")
                return value == string.Empty ? "Resources/Images/empty_cover.jpg" : $"https://elemsocial.com/Content/Simple/{value}";

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

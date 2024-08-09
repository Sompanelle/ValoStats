using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace ValoStats.ViewModels.Converters;

public class ResultToStyleConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if ((bool?)value == true)
                return Brushes.ForestGreen;
        else if ((bool?)value == null)
                return Brushes.Gray;
        else
        return Brushes.DarkRed;

    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
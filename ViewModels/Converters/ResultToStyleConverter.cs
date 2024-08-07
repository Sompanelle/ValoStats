using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace ValoStats.ViewModels.Converters;

public class ResultToStyleConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var lossBrush = Brushes.Salmon;
        var winBrush = Brushes.ForestGreen;
        if (value == "Win")
                return winBrush;
        else
                return lossBrush;
        
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
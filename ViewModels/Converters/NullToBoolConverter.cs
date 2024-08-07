using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace ValoStats.ViewModels.Converters;

public class NullToBoolConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null || (bool)value == false)
        {
            return false;
        }
        else return true;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
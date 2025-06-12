using System;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml;

namespace WinUI3_MVVM_Project.Converters;
public class NullToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        bool isNull = value == null;
        // Nếu parameter là "true" (string), đảo ngược logic: hiện khi null, ẩn khi không null
        bool inverse = (parameter as string)?.Equals("true", StringComparison.OrdinalIgnoreCase) ?? false;

        if (inverse)
        {
            return isNull ? Visibility.Visible : Visibility.Collapsed;
        }
        return isNull ? Visibility.Collapsed : Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
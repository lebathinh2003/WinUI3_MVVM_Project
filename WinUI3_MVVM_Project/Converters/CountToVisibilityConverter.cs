using System;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml;

namespace WinUI3_MVVM_Project.Converters;
public class CountToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is int count)
        {
            bool isEmpty = count == 0;
            // Nếu parameter là "true" (string), đảo ngược logic: hiện khi trống, ẩn khi không trống
            bool inverse = (parameter as string)?.Equals("true", StringComparison.OrdinalIgnoreCase) ?? false;
            if (inverse)
            {
                return isEmpty ? Visibility.Visible : Visibility.Collapsed;
            }
            return isEmpty ? Visibility.Collapsed : Visibility.Visible;
        }
        return Visibility.Collapsed;
    }
    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}
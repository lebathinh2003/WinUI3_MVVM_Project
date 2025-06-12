using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;

namespace WinUI3_MVVM_Project.Converters;

public class BooleanToVisibilityConverter : Microsoft.UI.Xaml.Data.IValueConverter
{
    public object Convert(object value, System.Type targetType, object parameter, string language)
    {
        bool boolValue = (bool)value;
        bool inverse = (parameter as string)?.ToLower() == "true"; // Check for "True" to inverse

        if (inverse)
        {
            boolValue = !boolValue;
        }

        return boolValue ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, System.Type targetType, object parameter, string language)
    {
        throw new System.NotImplementedException();
    }
}

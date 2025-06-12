// Converters/ViewModelToViewConverter.cs
using Microsoft.UI.Xaml.Data;
using System;
// Thêm các using directives cho namespaces của ViewModels và Views của bạn
using WinUI3_MVVM_Project.ViewModels;
using WinUI3_MVVM_Project.Views; // Giả sử các Page của bạn nằm trong thư mục Views

namespace WinUI3_MVVM_Project.Converters;

public class ViewModelToViewConverter : IValueConverter
{
    /// <summary>
    /// Chuyển đổi một ViewModel thành một View (Page instance) tương ứng.
    /// </summary>
    /// <param name="value">ViewModel instance (ví dụ: LoginViewModel).</param>
    /// <param name="targetType">Kiểu mong muốn của target (không dùng ở đây).</param>
    /// <param name="parameter">Tham số bổ sung (không dùng ở đây).</param>
    /// <param name="language">Ngôn ngữ (không dùng ở đây).</param>
    /// <returns>Một instance của Page tương ứng hoặc null nếu không tìm thấy mapping.</returns>
    public object? Convert(object value, Type targetType, object parameter, string language)
    {
        // 'value' là instance của ViewModel (ví dụ: CurrentViewModel từ MainViewModel)
        return value switch
        {
            LoginViewModel => new LoginPage(),
            ProfileViewModel => new ProfilePage(),
            UserManagementViewModel => new UserManagementPage(),
            // Thêm các ánh xạ ViewModel -> View khác tại đây nếu cần
            _ => null // Hoặc trả về một trang lỗi mặc định, hoặc throw ArgumentException
        };
        // Lưu ý: Cách này sẽ tạo một instance MỚI của Page mỗi khi ViewModel thay đổi.
        // Nếu bạn cần cache các instance của Page hoặc quản lý trạng thái Page phức tạp hơn,
        // bạn có thể cần một service điều hướng (navigation service) hoặc một cơ chế cache phức tạp hơn.
        // Đối với nhiều ứng dụng, việc tạo instance mới là chấp nhận được.
    }

    /// <summary>
    /// Chuyển đổi ngược từ View về ViewModel (thường không cần thiết cho converter này).
    /// </summary>
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        // Converter này chủ yếu dùng cho one-way binding để hiển thị content.
        throw new NotImplementedException("Không thể chuyển đổi View về ViewModel bằng converter này.");
    }
}

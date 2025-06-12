using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using WinUI3_MVVM_Project.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUI3_MVVM_Project.Views;
public sealed partial class LoginPage : Page
{
    // ViewModel cho Page này.
    public LoginViewModel ViewModel { get; }

    public LoginPage()
    {
        this.InitializeComponent();

        // Lấy LoginViewModel từ DI container (App.Services)
        // Đảm bảo LoginViewModel đã được đăng ký trong App.xaml.cs
        ViewModel = App.Current.Services.GetService<LoginViewModel>()!;
        if (ViewModel == null)
        {
            // Xử lý lỗi nếu không lấy được ViewModel.
            // Điều này không nên xảy ra nếu DI được cấu hình đúng.
            throw new System.InvalidOperationException("Could not resolve LoginViewModel from services. Check DI configuration.");
        }

        // Gán DataContext của Page cho ViewModel của nó.
        // Điều này cho phép các binding {x:Bind ViewModel...} trong XAML hoạt động.
        this.DataContext = ViewModel;
    }
}
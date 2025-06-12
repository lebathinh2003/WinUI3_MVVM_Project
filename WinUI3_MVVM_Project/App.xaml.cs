using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using WinUI3_MVVM_Project.Services;
using WinUI3_MVVM_Project.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUI3_MVVM_Project;
public partial class App : Application
{
    /// <summary>
    /// Lấy instance hiện tại của <see cref="App"/>.
    /// </summary>
    public new static App Current => (App)Application.Current;

    /// <summary>
    /// Lấy instance <see cref="IServiceProvider"/> cho ứng dụng.
    /// </summary>
    public IServiceProvider Services { get; }

    public App()
    {
        this.InitializeComponent();
        Services = ConfigureServices();
    }

    /// <summary>
    /// Cấu hình các services cho ứng dụng.
    /// </summary>
    private static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        // Đăng ký Services
        services.AddSingleton<IIdentityService, IdentityService>();
        services.AddSingleton<IUserService, UserService>();

        // Đăng ký ViewModels
        services.AddSingleton<MainViewModel>();
        services.AddSingleton<LoginViewModel>();
        services.AddSingleton<ProfileViewModel>();
        services.AddSingleton<UserManagementViewModel>();

        // Các Views (Pages) thường không cần đăng ký ở đây khi dùng ViewModelToViewConverter
        // vì converter sẽ tạo instance mới của Page.

        return services.BuildServiceProvider();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        m_window = new MainWindow();
        // Kích hoạt cửa sổ.
        // Đảm bảo rằng cửa sổ hiện tại được kích hoạt.
        m_window.Activate();
    }

    private Window? m_window;
    public Window? MainWindowInstance => m_window;
}
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using System.Threading.Tasks;
using System.Windows.Input;
using WinUI3_MVVM_Project.Services;
using WinUI3_MVVM_Project.Views;
namespace WinUI3_MVVM_Project.ViewModels;
public partial class MainViewModel : ViewModelBase // Thêm 'partial'
{
    private readonly IIdentityService _identityService;
    private readonly LoginViewModel _loginViewModel;
    private readonly ProfileViewModel _profileViewModel;
    private readonly UserManagementViewModel _userManagementViewModel;

    [ObservableProperty]
    private ViewModelBase? _currentViewModel;

    [ObservableProperty]
    // [NotifyCanExecuteChangedFor(nameof(NavigateToProfileCommand))] // Tự động thông báo cho các command
    // [NotifyCanExecuteChangedFor(nameof(NavigateToUserManagementCommand))]
    // [NotifyCanExecuteChangedFor(nameof(LogoutCommand))]
    // Thay vì các dòng trên, RelayCommand sẽ tự theo dõi IsUserLoggedIn nếu nó là [ObservableProperty]
    private bool _isUserLoggedIn;

    [ObservableProperty]
    private object? _selectedNavigationViewItem;

    // Các command sẽ được tạo bởi source generator

    public MainViewModel(
        IIdentityService identityService,
        LoginViewModel loginViewModel,
        ProfileViewModel profileViewModel,
        UserManagementViewModel userManagementViewModel)
    {
        _identityService = identityService;
        _loginViewModel = loginViewModel;
        _profileViewModel = profileViewModel;
        _userManagementViewModel = userManagementViewModel;

        _loginViewModel.LoginSuccess += HandleLoginSuccessAsync;

        // Khởi tạo ban đầu
        CurrentViewModel = _loginViewModel;
        // IsUserLoggedIn mặc định là false (từ kiểu bool)
    }

    private async Task HandleLoginSuccessAsync()
    {
        IsUserLoggedIn = true; // Đây là [ObservableProperty], sẽ tự thông báo thay đổi
        await ExecuteNavigateToProfileAsync(); // Gọi phiên bản async của điều hướng profile
    }

    // CanExecute cho các lệnh điều hướng và logout
    private bool CanExecuteIfLoggedIn() => IsUserLoggedIn;

    [RelayCommand(CanExecute = nameof(CanExecuteIfLoggedIn))]
    private async Task ExecuteNavigateToProfileAsync() // Đổi tên để rõ ràng hơn là có thể async
    {
        CurrentViewModel = _profileViewModel;
        // Gọi lệnh tải dữ liệu cho ProfileViewModel
        // LoadUserProfileCommand là RelayCommand trong ProfileViewModel
        if (_profileViewModel.LoadUserProfileCommand.CanExecute(null))
        {
            await _profileViewModel.LoadUserProfileCommand.ExecuteAsync(null);
        }
    }

    [RelayCommand(CanExecute = nameof(CanExecuteIfLoggedIn))]
    private async Task ExecuteNavigateToUserManagementAsync() // Đổi tên
    {
        CurrentViewModel = _userManagementViewModel;
        await _userManagementViewModel.InitializeAsync();
    }

    [RelayCommand(CanExecute = nameof(CanExecuteIfLoggedIn))]
    private async Task ExecuteLogoutAsync()
    {
        await _identityService.LogoutAsync();
        IsUserLoggedIn = false;
        CurrentViewModel = _loginViewModel;
        SelectedNavigationViewItem = null;

        _loginViewModel.Username = string.Empty;
        _loginViewModel.Password = string.Empty;
        _loginViewModel.ErrorMessage = string.Empty;
    }

    async partial void OnSelectedNavigationViewItemChanged(object? oldValue, object? newValue)
    {
        if (!IsUserLoggedIn && newValue != null)
        {
            // Nếu người dùng chưa đăng nhập nhưng có một mục được chọn (ví dụ: trạng thái được khôi phục),
            // hãy bỏ qua hoặc đặt lại để tránh điều hướng không mong muốn.
            // Hoặc, bạn có thể muốn xóa SelectedNavigationViewItem để nó không còn được chọn.
            // _selectedNavigationViewItem = null; // Trực tiếp gán vào field để không trigger lại
            // OnPropertyChanged(nameof(SelectedNavigationViewItem));
            return;
        }

        if (newValue is NavigationViewItem selectedItem)
        {
            var tag = selectedItem.Tag?.ToString();

            switch (tag)
            {
                case nameof(ProfilePage):
                    if (ExecuteNavigateToProfileCommand.CanExecute(null))
                    {
                        await ExecuteNavigateToProfileCommand.ExecuteAsync(null);
                    }
                    break;
                case nameof(UserManagementPage):
                    if (ExecuteNavigateToUserManagementCommand.CanExecute(null))
                    {
                        await ExecuteNavigateToUserManagementCommand.ExecuteAsync(null);
                    }
                    break;
                case "Logout":
                    if (ExecuteLogoutCommand.CanExecute(null))
                    {
                        await ExecuteLogoutCommand.ExecuteAsync(null);
                    }
                    break;
            }
        }
    }

    // Ghi đè OnIsUserLoggedInChanged để cập nhật CanExecute cho các command
    // nếu RelayCommand không tự động theo dõi IsUserLoggedIn một cách chính xác.
    // Tuy nhiên, với [ObservableProperty] trên IsUserLoggedIn,
    // các RelayCommand có CanExecute = nameof(CanExecuteIfLoggedIn)
    // nên tự động cập nhật. Nếu không, bạn có thể thêm:
    // partial void OnIsUserLoggedInChanged(bool value)
    // {
    //     NavigateToProfileCommand.NotifyCanExecuteChanged();
    //     NavigateToUserManagementCommand.NotifyCanExecuteChanged();
    //     LogoutCommand.NotifyCanExecuteChanged();
    // }
}

// ViewModels/LoginViewModel.cs
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using WinUI3_MVVM_Project.Models;
using WinUI3_MVVM_Project.Services;
using System; // For Func<Task>
using System.ComponentModel; // For PropertyChangedEventArgs

namespace WinUI3_MVVM_Project.ViewModels
{
    public partial class LoginViewModel : ViewModelBase // Thêm 'partial'
    {
        private readonly IIdentityService _identityService;

        [ObservableProperty]
        private string? _username;

        [ObservableProperty]
        private string? _password;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasErrorMessage))]
        private string? _errorMessage;

        public bool HasErrorMessage => !string.IsNullOrEmpty(ErrorMessage);

        public event Func<Task>? LoginSuccess;

        public LoginViewModel(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        // Phương thức CanExecute cho LoginCommand
        private bool CanExecuteLogin()
        {
            return !string.IsNullOrWhiteSpace(Username) &&
                   !string.IsNullOrWhiteSpace(Password) &&
                   !IsLoading; // Phụ thuộc vào IsLoading từ ViewModelBase
        }

        // Source generator sẽ tạo LoginCommand từ phương thức này
        [RelayCommand(CanExecute = nameof(CanExecuteLogin))]
        private async Task ExecuteLoginAsync()
        {
            IsLoading = true;
            ErrorMessage = null;

            try
            {
                var loginRequest = new LoginRequest { Username = Username, Password = Password };
                User? user = await _identityService.LoginAsync(loginRequest);

                if (user != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Login successful for user: {user.Username}");
                    if (LoginSuccess != null)
                    {
                        await LoginSuccess.Invoke();
                    }
                }
                else
                {
                    ErrorMessage = "Tên đăng nhập hoặc mật khẩu không đúng.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Đã xảy ra lỗi: {ex.Message}";
                System.Diagnostics.Debug.WriteLine($"Login failed: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
                // Khi IsLoading (là [ObservableProperty]) thay đổi, CanExecuteLogin sẽ tự động được đánh giá lại
                // bởi RelayCommand vì CanExecuteLogin phụ thuộc vào nó.
                // Việc gọi NotifyCanExecuteChanged() một cách tường minh ở đây thường không cần thiết
                // cho IsLoading, nhưng đã được thêm cho Username và Password thông qua các phương thức On[Property]Changed.
            }
        }

        // Phương thức partial được gọi khi Username thay đổi (tạo bởi [ObservableProperty])
        partial void OnUsernameChanged(string? oldValue, string? newValue)
        {
            ExecuteLoginCommand.NotifyCanExecuteChanged();
        }

        // Phương thức partial được gọi khi Password thay đổi
        partial void OnPasswordChanged(string? oldValue, string? newValue)
        {
            ExecuteLoginCommand.NotifyCanExecuteChanged();
        }


        // Ghi đè phương thức OnPropertyChanged để theo dõi sự thay đổi của IsLoading từ ViewModelBase
        // và thông báo cho LoginCommand cập nhật trạng thái CanExecute một cách tường minh.
        // Mặc dù RelayCommand thường tự động xử lý điều này cho các [ObservableProperty]
        // mà CanExecute phụ thuộc vào, cách này đảm bảo nó hoạt động.
        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e); // Gọi triển khai của lớp cơ sở

            // Kiểm tra xem thuộc tính đã thay đổi có phải là IsLoading không
            if (e.PropertyName == nameof(IsLoading))
            {
                // Thông báo cho LoginCommand rằng điều kiện CanExecute của nó có thể đã thay đổi
                // LoginCommand được tạo bởi source generator từ [RelayCommand]
                ExecuteLoginCommand.NotifyCanExecuteChanged();
            }
        }
    }
}

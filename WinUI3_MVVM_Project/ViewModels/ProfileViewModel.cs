using System.Threading.Tasks;
using WinUI3_MVVM_Project.Models;
using WinUI3_MVVM_Project.Services;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace WinUI3_MVVM_Project.ViewModels;
public partial class ProfileViewModel : ViewModelBase // Thêm 'partial'
{
    private readonly IIdentityService _identityService;

    [ObservableProperty]
    private User? _currentUser;

    // LoadUserProfileCommand sẽ được tạo bởi source generator
    // từ phương thức LoadUserProfileAsync.
    // Không có CanExecute vì nó luôn có thể thực thi (hoặc không cần kiểm tra).

    public ProfileViewModel(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    [RelayCommand] // Sử dụng RelayCommand vì nó có thể không luôn là async nếu dữ liệu đã được cache
    private async Task LoadUserProfileAsync()
    {
        IsLoading = true;
        try
        {
            // CurrentUser là một [ObservableProperty], gán giá trị sẽ tự động thông báo thay đổi.
            CurrentUser = await _identityService.GetCurrentUserAsync();
            if (CurrentUser == null)
            {
                System.Diagnostics.Debug.WriteLine("ProfileViewModel: No current user found after attempting to load.");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"ProfileViewModel: Error loading user profile: {ex.Message}");
            // Cân nhắc việc thêm một ErrorMessage property vào ViewModelBase hoặc ProfileViewModel
            // để hiển thị lỗi này trên UI nếu cần.
            // ErrorMessage = $"Không thể tải thông tin người dùng: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }
}
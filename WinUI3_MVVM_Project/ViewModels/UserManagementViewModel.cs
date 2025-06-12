// WinUI3_MVVM_Project.ViewModels.UserManagementViewModel.cs
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WinUI3_MVVM_Project.Models;
using WinUI3_MVVM_Project.Services;
using System;
using WinUI3_MVVM_Project.Interfaces;
using System.Windows.Input;
using System.Linq;
using System.Diagnostics; // For Debug.WriteLine

namespace WinUI3_MVVM_Project.ViewModels
{
    public partial class UserManagementViewModel : ViewModelBase, ITablePaginationControl
    {
        private readonly IUserService _userService;
        public ObservableCollection<User> Users { get; } = new ObservableCollection<User>();

        // MỚI: Thuộc tính để lưu giữ User đang được chọn
        [ObservableProperty]
        private User? _selectedUser;

        // MỚI: Command được gọi từ UI khi một User được click
        [RelayCommand]
        private void SelectUser(User? user)
        {
            // Cập nhật User đang được chọn
            SelectedUser = user;

            // Bạn có thể thêm logic khác ở đây, ví dụ:
            // Cập nhật trạng thái của các nút Edit/Delete
            Debug.WriteLine($"User selected: {user?.FullName ?? "None"}");
        }

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CurrentPageInfo))]
        private int _currentPage = 1;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CurrentPageInfo))]
        private int _totalPages = 1;

        public string CurrentPageInfo => $"Trang {CurrentPage}/{TotalPages}";
   
        public ICommand FirstPageCommand { get; }
        public ICommand PreviousPageCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand LastPageCommand { get; }

        public UserManagementViewModel(IUserService userService)
        {
            _userService = userService;
            // Khởi tạo giá trị ban đầu trước khi gán command để CanExecute có thể đọc giá trị đúng
            _currentPage = 1;
            _totalPages = 1;

            FirstPageCommand = new AsyncRelayCommand(GoToFirstPage, CanGoToFirstPage);
            PreviousPageCommand = new AsyncRelayCommand(GoToPreviousPage, CanGoToPreviousPage);
            NextPageCommand = new AsyncRelayCommand(GoToNextPage, CanGoToNextPage);
            LastPageCommand = new AsyncRelayCommand(GoToLastPage, CanGoToLastPage);

            NotifyCommandCanExecuteChanged();  // Cập nhật trạng thái button ban đầu
        }

 

        // Khi CurrentPage (được tạo bởi [ObservableProperty] từ _currentPage) thay đổi
        partial void OnCurrentPageChanged(int oldValue, int newValue)
        {
            NotifyCommandCanExecuteChanged();
        }

        // Khi TotalPages (được tạo bởi [ObservableProperty] từ _totalPages) thay đổi
        partial void OnTotalPagesChanged(int oldValue, int newValue)
        {
            NotifyCommandCanExecuteChanged();
        }

        private void NotifyCommandCanExecuteChanged()
        {
            (FirstPageCommand as IRelayCommand)?.NotifyCanExecuteChanged();
            (PreviousPageCommand as IRelayCommand)?.NotifyCanExecuteChanged();
            (NextPageCommand as IRelayCommand)?.NotifyCanExecuteChanged();
            (LastPageCommand as IRelayCommand)?.NotifyCanExecuteChanged();
        }

        public async Task InitializeAsync()
        {
            CurrentPage = 1; // Đảm bảo trang hiện tại là 1 khi bắt đầu
            await LoadUsersAsync();
            NotifyCommandCanExecuteChanged();
        }

        protected virtual async Task GoToFirstPage()
        {
            if (CanGoToFirstPage()) CurrentPage = 1;
            await LoadUsersPagedAsync();
        }
        protected virtual bool CanGoToFirstPage() => CurrentPage > 1;

        protected virtual async Task GoToPreviousPage()
        {
            if (CanGoToPreviousPage()) CurrentPage--;
            await LoadUsersPagedAsync();
        }
        protected virtual bool CanGoToPreviousPage() => CurrentPage > 1;

        protected virtual async Task GoToNextPage()
        {
            if (CanGoToNextPage()) CurrentPage++;
            await LoadUsersPagedAsync();
        }
        protected virtual bool CanGoToNextPage() => CurrentPage < TotalPages;

        protected virtual async Task GoToLastPage()
        {
            if (CanGoToLastPage()) CurrentPage = TotalPages;
            await LoadUsersPagedAsync();
        }
        protected virtual bool CanGoToLastPage() => CurrentPage < TotalPages;

        [RelayCommand]
        private async Task LoadUsersAsync()
        {
            IsLoading = true;
            Users.Clear(); // Xóa dữ liệu cũ trước khi tải mới
            try
            {
                var usersList = await _userService.GetUsersAsync();
                if (usersList != null && usersList.Any())
                {
                    int totalCount = usersList.Count();
                    int itemsPerPage = 10;
                    int newTotalPages = (int)Math.Ceiling((double)totalCount / itemsPerPage);
                    TotalPages = newTotalPages > 0 ? newTotalPages : 1; // Đảm bảo TotalPages luôn >= 1

                    // CurrentPage nên đã được set (ví dụ: 1 trong InitializeAsync)
                    // Chỉ tải dữ liệu cho trang hiện tại
                    var pagedUsers = usersList
                                       .Skip((CurrentPage - 1) * itemsPerPage)
                                       .Take(itemsPerPage);
                    foreach (var user in pagedUsers) Users.Add(user);
                }
                else
                {
                    TotalPages = 1; // Nếu không có user, vẫn là 1 trang (trống)
                }
            }
            catch (Exception ex) { Debug.WriteLine($"LoadUsersAsync Error: {ex.Message}"); TotalPages = 1; }
            finally { IsLoading = false; 
                NotifyCommandCanExecuteChanged();
            } 
        }

        public async Task LoadUsersPagedAsync()
        {
            IsLoading = true;
            Users.Clear();
            try
            {
                var allUsers = await _userService.GetUsersAsync();
                if (allUsers != null && allUsers.Any())
                {
                    int itemsPerPage = 10;
                    // TotalPages nên được set chính xác từ trước, không cần tính lại ở đây nếu không có nguồn tổng item mới
                    var pagedUsers = allUsers
                                       .Skip((CurrentPage - 1) * itemsPerPage)
                                       .Take(itemsPerPage);
                    foreach (var user in pagedUsers) Users.Add(user);
                }
                // else Users collection is already cleared
            }
            catch (Exception ex) { Debug.WriteLine($"LoadUsersPagedAsync Error: {ex.Message}"); }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
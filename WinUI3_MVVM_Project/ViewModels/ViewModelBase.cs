using CommunityToolkit.Mvvm.ComponentModel;

namespace WinUI3_MVVM_Project.ViewModels;
public abstract partial class ViewModelBase : ObservableObject // Thêm 'partial' để source generators hoạt động
{
    // Source generator sẽ tạo thuộc tính IsLoading từ trường _isLoading này,
    // bao gồm cả logic SetProperty và OnPropertyChanged.
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotLoading))] // Thông báo cho IsNotLoading khi IsLoading thay đổi
    private bool _isLoading;

    /// <summary>
    /// Thuộc tính phụ thuộc, tiện lợi cho binding ngược lại của IsLoading.
    /// </summary>
    public bool IsNotLoading => !IsLoading;
}
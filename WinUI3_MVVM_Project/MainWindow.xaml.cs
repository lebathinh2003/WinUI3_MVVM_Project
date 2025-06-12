// MainWindow.xaml.cs
using Microsoft.UI.Xaml;
// using WinUI3_MVVM_Project.Views; // Bỏ comment nếu các Page của bạn nằm trong thư mục Views

namespace WinUI3_MVVM_Project;
public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        this.InitializeComponent();

        // Thiết lập tiêu đề cho cửa sổ
        Title = "WinUI 3 MVVM Project";

        // Kích thước cửa sổ ban đầu (tùy chọn)
        // AppWindow.Resize(new Windows.Graphics.SizeInt32(1024, 768));

        // Các thiết lập khác cho Window có thể được đặt ở đây nếu cần.
        // Ví dụ: ExtendsContentIntoTitleBar = true; SetTitleBar(AppTitleBar);
    }
}
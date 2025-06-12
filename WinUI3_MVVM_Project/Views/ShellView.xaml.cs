using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WinUI3_MVVM_Project.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUI3_MVVM_Project.Views;
public sealed partial class ShellView : UserControl
{
    // ViewModel cho ShellView, chính là MainViewModel
    public MainViewModel ViewModel { get; }

    public ShellView()
    {
        this.InitializeComponent();

        // Lấy MainViewModel từ DI container
        ViewModel = App.Current.Services.GetService<MainViewModel>()!;
        if (ViewModel == null)
        {
            throw new System.InvalidOperationException("Could not resolve MainViewModel for ShellView. Check DI configuration.");
        }

        // Gán DataContext của UserControl này cho chính nó,
        // để các binding {x:Bind ViewModel...} trong XAML có thể truy cập thuộc tính ViewModel.
        this.DataContext = this;
    }
}

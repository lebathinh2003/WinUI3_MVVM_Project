using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinUI3_MVVM_Project.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUI3_MVVM_Project.Views;
public sealed partial class UserManagementPage : Page
{
    public UserManagementViewModel ViewModel { get; }

    public UserManagementPage()
    {
        this.InitializeComponent();

        ViewModel = App.Current.Services.GetService<UserManagementViewModel>()!;
        if (ViewModel == null)
        {
            throw new System.InvalidOperationException("Could not resolve UserManagementViewModel from services.");
        }
        this.DataContext = ViewModel;

        // MainViewModel sẽ gọi LoadUsersCommand khi điều hướng đến trang này.
    }
}
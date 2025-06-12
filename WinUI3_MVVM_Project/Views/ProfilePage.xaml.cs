using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using WinUI3_MVVM_Project.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUI3_MVVM_Project.Views
{
    public sealed partial class ProfilePage : Page
    {
        public ProfileViewModel ViewModel { get; }

        public ProfilePage()
        {
            this.InitializeComponent();

            ViewModel = App.Current.Services.GetService<ProfileViewModel>()!;
            if (ViewModel == null)
            {
                throw new System.InvalidOperationException("Could not resolve ProfileViewModel from services.");
            }
            this.DataContext = ViewModel;
        }
    }
}

using CommunityToolkit.Mvvm.ComponentModel;

namespace WinUI3_MVVM_Project.Models;
public partial class User : ObservableObject
{

    [ObservableProperty]
    private int _id;

    [ObservableProperty]
    private string? _username;

    [ObservableProperty]
    private string? _fullName;

    [ObservableProperty]
    private string? _email;

}

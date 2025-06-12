using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinUI3_MVVM_Project.Models;

namespace WinUI3_MVVM_Project.Services;
public class IdentityService : IIdentityService
{
    private readonly User _registeredUser = new User
    {
        Id = 1,
        Username = "testuser",
        FullName = "Test User",
        Email = "test@example.com"
    };
    private User? _currentUser;

    public async Task<User?> LoginAsync(LoginRequest loginRequest)
    {
        await Task.Delay(500); // Giả lập độ trễ mạng

        if (loginRequest.Username == _registeredUser.Username && loginRequest.Password == "password") // Mật khẩu giả lập
        {
            _currentUser = _registeredUser;
            return _currentUser;
        }
        return null;
    }

    public async Task LogoutAsync()
    {
        _currentUser = null;
        await Task.CompletedTask; 
    }

    public async Task<User?> GetCurrentUserAsync()
    {
        await Task.CompletedTask;
        return _currentUser;
    }
}

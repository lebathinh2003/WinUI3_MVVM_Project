using System.Threading.Tasks;
using WinUI3_MVVM_Project.Models;

namespace WinUI3_MVVM_Project.Services;

public interface IIdentityService
{
    Task<User?> LoginAsync(LoginRequest loginRequest);
    Task LogoutAsync();
    Task<User?> GetCurrentUserAsync();
}

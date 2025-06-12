using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinUI3_MVVM_Project.Models;

namespace WinUI3_MVVM_Project.Services;

public interface IUserService
{
    Task<IEnumerable<User>> GetUsersAsync();

    Task<User?> GetUserByIdAsync(int userId);
}

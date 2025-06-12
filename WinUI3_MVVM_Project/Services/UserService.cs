using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WinUI3_MVVM_Project.Models;

namespace WinUI3_MVVM_Project.Services;

public class UserService : IUserService
{
    private readonly List<User> _users = new List<User>
    {
        // Original 3 users
        new User { Id = 1, Username = "testuser", FullName = "Test User", Email = "test@example.com" },
        new User { Id = 2, Username = "admin", FullName = "Administrator", Email = "admin@example.com" },
        new User { Id = 3, Username = "guest", FullName = "Guest User", Email = "guest@example.com" },
        new User { Id = 4, Username = "user1", FullName = "User One", Email = "user1@example.com" },
        new User { Id = 5, Username = "user2", FullName = "User Two", Email = "user2@example.com" },
        new User { Id = 6, Username = "user3", FullName = "User Three", Email = "user3@example.com" },
        new User { Id = 7, Username = "user4", FullName = "User Four", Email = "user4@example.com" },
        new User { Id = 8, Username = "user5", FullName = "User Five", Email = "user5@example.com" },
        new User { Id = 9, Username = "user6", FullName = "User Six", Email = "user6@example.com" },
        new User { Id = 10, Username = "user7", FullName = "User Seven", Email = "user7@example.com" },
        new User { Id = 11, Username = "user8", FullName = "User Eight", Email = "user8@example.com" },
        new User { Id = 12, Username = "user9", FullName = "User Nine", Email = "user9@example.com" },
        new User { Id = 13, Username = "user10", FullName = "User Ten", Email = "user10@example.com" },
        new User { Id = 14, Username = "user11", FullName = "User Eleven", Email = "user11@example.com" },
        new User { Id = 15, Username = "user12", FullName = "User Twelve", Email = "user12@example.com" },
        new User { Id = 16, Username = "user13", FullName = "User Thirteen", Email = "user13@example.com" },
        new User { Id = 17, Username = "user14", FullName = "User Fourteen", Email = "user14@example.com" },
        new User { Id = 18, Username = "user15", FullName = "User Fifteen", Email = "user15@example.com" },
        new User { Id = 19, Username = "user16", FullName = "User Sixteen", Email = "user16@example.com" },
        new User { Id = 20, Username = "user17", FullName = "User Seventeen", Email = "user17@example.com" },
        new User { Id = 21, Username = "user18", FullName = "User Eighteen", Email = "user18@example.com" },
        new User { Id = 22, Username = "user19", FullName = "User Nineteen", Email = "user19@example.com" },
        new User { Id = 23, Username = "user20", FullName = "User Twenty", Email = "user20@example.com" },
        new User { Id = 24, Username = "user21", FullName = "User Twenty-One", Email = "user21@example.com" },
        new User { Id = 25, Username = "user22", FullName = "User Twenty-Two", Email = "user22@example.com" },
        new User { Id = 26, Username = "user23", FullName = "User Twenty-Three", Email = "user23@example.com" },
        new User { Id = 27, Username = "user24", FullName = "User Twenty-Four", Email = "user24@example.com" },
        new User { Id = 28, Username = "user25", FullName = "User Twenty-Five", Email = "user25@example.com" },
        new User { Id = 29, Username = "user26", FullName = "User Twenty-Six", Email = "user26@example.com" },
        new User { Id = 30, Username = "user27", FullName = "User Twenty-Seven", Email = "user27@example.com" },
        new User { Id = 31, Username = "user28", FullName = "User Twenty-Eight", Email = "user28@example.com" },
        new User { Id = 32, Username = "user29", FullName = "User Twenty-Nine", Email = "user29@example.com" },
        new User { Id = 33, Username = "user30", FullName = "User Thirty", Email = "user30@example.com" },
        new User { Id = 34, Username = "user31", FullName = "User Thirty-One", Email = "user31@example.com" },
        new User { Id = 35, Username = "user32", FullName = "User Thirty-Two", Email = "user32@example.com" },
        new User { Id = 36, Username = "user33", FullName = "User Thirty-Three", Email = "user33@example.com" },
        new User { Id = 37, Username = "user34", FullName = "User Thirty-Four", Email = "user34@example.com" },
        new User { Id = 38, Username = "user35", FullName = "User Thirty-Five", Email = "user35@example.com" },
        new User { Id = 39, Username = "user36", FullName = "User Thirty-Six", Email = "user36@example.com" },
        new User { Id = 40, Username = "user37", FullName = "User Thirty-Seven", Email = "user37@example.com" },
        new User { Id = 41, Username = "user38", FullName = "User Thirty-Eight", Email = "user38@example.com" },
        new User { Id = 42, Username = "user39", FullName = "User Thirty-Nine", Email = "user39@example.com" },
        new User { Id = 43, Username = "user40", FullName = "User Forty", Email = "user40@example.com" },
        new User { Id = 44, Username = "user41", FullName = "User Forty-One", Email = "user41@example.com" },
        new User { Id = 45, Username = "user42", FullName = "User Forty-Two", Email = "user42@example.com" },
        new User { Id = 46, Username = "user43", FullName = "User Forty-Three", Email = "user43@example.com" },
        new User { Id = 47, Username = "user44", FullName = "User Forty-Four", Email = "user44@example.com" },
        new User { Id = 48, Username = "user45", FullName = "User Forty-Five", Email = "user45@example.com" },
        new User { Id = 49, Username = "user46", FullName = "User Forty-Six", Email = "user46@example.com" },
        new User { Id = 50, Username = "user47", FullName = "User Forty-Seven", Email = "user47@example.com" },
        new User { Id = 51, Username = "user48", FullName = "User Forty-Eight", Email = "user48@example.com" },
        new User { Id = 52, Username = "user49", FullName = "User Forty-Nine", Email = "user49@example.com" },
        new User { Id = 53, Username = "user50", FullName = "User Fifty", Email = "user50@example.com" },
        new User { Id = 54, Username = "user51", FullName = "User Fifty-One", Email = "user51@example.com" },
        new User { Id = 55, Username = "user52", FullName = "User Fifty-Two", Email = "user52@example.com" },
        new User { Id = 56, Username = "user53", FullName = "User Fifty-Three", Email = "user53@example.com" },
        new User { Id = 57, Username = "user54", FullName = "User Fifty-Four", Email = "user54@example.com" },
        new User { Id = 58, Username = "user55", FullName = "User Fifty-Five", Email = "user55@example.com" },
        new User { Id = 59, Username = "user56", FullName = "User Fifty-Six", Email = "user56@example.com" },
        new User { Id = 60, Username = "user57", FullName = "User Fifty-Seven", Email = "user57@example.com" },
        new User { Id = 61, Username = "user58", FullName = "User Fifty-Eight", Email = "user58@example.com" },
        new User { Id = 62, Username = "user59", FullName = "User Fifty-Nine", Email = "user59@example.com" },
        new User { Id = 63, Username = "user60", FullName = "User Sixty", Email = "user60@example.com" }
    };

    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        await Task.Delay(300); // Giả lập độ trễ mạng
        return _users;
    }

    public async Task<User?> GetUserByIdAsync(int userId)
    {
        await Task.Delay(100); // Giả lập độ trễ mạng
        return _users.FirstOrDefault(u => u.Id == userId);
    }
}

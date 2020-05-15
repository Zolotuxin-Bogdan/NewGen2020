using Gallery.BLL.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gallery.DAL;

namespace Gallery.BLL
{
    public interface IUsersService
    {
        Task<UserDTO> FindUserAsync(string username, string password);

        Task<bool> IsUserExistAsync(string username, string password);

        Task<bool> IsUserExistAsync(string username);

        Task RegisterUserAsync(CreateUserDTO dto);

        int GetUserId(string userName);

        string GetUserName(int id);

        Task<User> GetUserByIdAsync(int id);

        Task RegisterLoginAttemptToDatabaseAsync(LoginAttemptDTO dto);
    }
}

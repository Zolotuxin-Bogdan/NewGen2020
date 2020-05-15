using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.DAL
{
    public interface IRepository
    {
        Task<bool> IsUserExistAsync(string username, string password);

        Task RegisterUserToDatabaseAsync(string username, string password);

        int GetUserId(string userName);

        string GetUserName(int id);

        Task<User> GetUserByIdAsync(int id);

        Task RegisterLoginAttemptToDatabaseAsync(string email, string ipAddress, bool isSuccess);
    }
}

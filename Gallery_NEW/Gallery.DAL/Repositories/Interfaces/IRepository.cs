using System.Threading.Tasks;

namespace Gallery.DAL.Repositories.Interfaces
{
    public interface IRepository
    {
        Task<bool> IsUserExistAsync(string username, string password);

        Task<bool> IsUserExistAsync(string username);

        Task RegisterUserToDatabaseAsync(string username, string password);

        int GetUserId(string userName);

        string GetUserName(int id);

        Task<User> GetUserByIdAsync(int id);

        Task RegisterLoginAttemptToDatabaseAsync(string email, string ipAddress, bool isSuccess);
    }
}

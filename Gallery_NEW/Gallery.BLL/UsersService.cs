using Gallery.BLL.Contracts;
using Gallery.DAL;
using System;
using System.Threading.Tasks;

namespace Gallery.BLL
{
    public class UsersService : IUsersService
    {
        private readonly IRepository _repo;
        public UsersService(IRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }
        public async Task<bool> IsUserExistAsync(string username, string password)
        {
            return await _repo.IsUserExistAsync(username, password);
        }
        public Task<UserDTO> FindUserAsync(string username, string password)
        {
            throw new NotImplementedException();
        }

        public async Task RegisterUserAsync(CreateUserDTO dto)
        {
            await _repo.RegisterUserToDatabase(dto.EMail, dto.Password);
        }

        public int GetUserId(string userName)
        {
            return _repo.GetUserId(userName);
        }

        public string GetUserName(int id)
        {
            return _repo.GetUserName(id);
        }
    }
}

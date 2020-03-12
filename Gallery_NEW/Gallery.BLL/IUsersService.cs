using Gallery.BLL.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.BLL
{
    public interface IUsersService
    {
        Task<UserDTO> FindUserAsync(string username, string password);

        Task<bool> IsUserExistAsync(string username, string password);

        Task RegisterUserAsync(CreateUserDTO dto);
    }
}

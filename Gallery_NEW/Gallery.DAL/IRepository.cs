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

       // We need to Register User in Database
       // Task<???> RegisterUserToDatabase(string username, string password);
    }
}

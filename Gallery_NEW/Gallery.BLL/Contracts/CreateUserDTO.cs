using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.BLL.Contracts
{
    public class CreateUserDTO
    {
        public CreateUserDTO(string email, string password)
        {
            EMail = email;
            Password = password;
        }
        public string EMail { get; }
        public string Password { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.BLL.Contracts
{
    public class LoginAttemptDTO
    {
        public LoginAttemptDTO(string email, string ipAddress, bool isSuccess)
        {
            Email = email;
            IpAddress = ipAddress;
            IsSuccess = isSuccess;
        }

        public string Email { get; }
        public string IpAddress { get; set; }
        public bool IsSuccess { get; set; }
    }
}

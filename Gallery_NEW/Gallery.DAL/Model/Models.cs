using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.DAL.Model
{
    public class LoginModel
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }

    public class RegisterModel
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }
}

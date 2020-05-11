using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gallery.DAL.Model;

namespace Gallery.DAL
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public ICollection<Role> Roles { get; set; } =
            new List<Role>();
        public ICollection<Media> Media { get; set; } = 
            new List<Media>();
        public ICollection<Attempt> Attempts { get; set; } =
            new List<Attempt>();

    }
}

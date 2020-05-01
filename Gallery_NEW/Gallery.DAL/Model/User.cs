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
        public string EMail { get; set; }
        public string Password { get; set; }

        public ICollection<Role> Roles { get; set; }
        public ICollection<Media> Medias { get; set; }
        public User()
        {
            Roles = new List<Role>();
            Medias = new List<Media>();
        }
    }
}

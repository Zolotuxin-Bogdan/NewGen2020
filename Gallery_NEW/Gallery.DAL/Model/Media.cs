using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.DAL.Model
{
    public class Media
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int MediaTypeId { get; set; }
        public MediaType Type { get; set; }

    }
}

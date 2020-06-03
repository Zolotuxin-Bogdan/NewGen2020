using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.BLL.Contracts
{
    public class ExifDTO
    {
        public string Title { get; set; }
        public string Manufacturer { get; set; }
        public string ModelOfCamera { get; set; }
        public string FileSize { get; set; }
        public string DateCreation { get; set; }
        public string DateUpload { get; set; }
    }
}

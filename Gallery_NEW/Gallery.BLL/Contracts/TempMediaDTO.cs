using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.BLL.Contracts
{
    public class TempMediaDTO
    {
        public int Id { get; set; }
        public string UniqName { get; set; }
        public bool IsLoading { get; set; }
        public bool IsSuccess { get; set; }
        public int UserId { get; set; }
    }
}

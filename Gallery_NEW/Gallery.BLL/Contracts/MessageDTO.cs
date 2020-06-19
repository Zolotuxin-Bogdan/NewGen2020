using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.BLL.Contracts
{
    public class MessageDTO
    {
        public int UserId;
        public string FileName;
        public byte[] FileBytes;
        public string MainPath;
        public string TempPath;

    }
}

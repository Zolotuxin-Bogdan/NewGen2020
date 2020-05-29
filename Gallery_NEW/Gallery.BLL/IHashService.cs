using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.BLL
{
    public interface IHashService
    {
        string ComputeSha256Hash(string rawData);
        string ComputeSha256Hash(byte[] rawData);
    }
}

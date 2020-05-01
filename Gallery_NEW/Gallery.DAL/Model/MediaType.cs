using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.DAL.Model
{
    public enum TypeOfMedia
    {
        Image, Video, Sound
    }
    public class MediaType
    {
        public int Id { get; set; }
        public TypeOfMedia Type { get; set; }


        public ICollection<Media> Medias { get; set; }
        public MediaType()
        {
            Medias = new List<Media>();
        }
    }
}

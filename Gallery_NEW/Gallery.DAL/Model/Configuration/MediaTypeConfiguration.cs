using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.DAL.Model.Configuration
{
    public class MediaTypeConfiguration : EntityTypeConfiguration<MediaType>
    {
        public MediaTypeConfiguration()
        {
            ToTable("MediaTypes").HasKey(p => p.Id).Property(p => p.Id).IsRequired().HasColumnName("MediaTypeId");

            Property(p => p.Type).IsRequired().HasMaxLength(30);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.DAL.Model.Configuration
{
    public class MediaConfiguration : EntityTypeConfiguration<Media>
    {
        public MediaConfiguration()
        {
            ToTable("Media").HasKey(p => p.Id).Property(p => p.Id).IsRequired().HasColumnName("MediaId");

            Property(p => p.Name).IsRequired().HasColumnType("varchar").HasMaxLength(255);

            HasRequired(p => p.Type)
                .WithMany(d => d.Media)
                .HasForeignKey(c => c.MediaTypeId);
        }
    }
}

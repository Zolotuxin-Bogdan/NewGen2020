using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.DAL.Model.Configuration
{
    public class TempMediaConfiguration : EntityTypeConfiguration<TempMedia>
    {
        public TempMediaConfiguration()
        {
            ToTable("TempMedia").HasKey(p => p.Id).Property(p => p.Id).IsRequired().HasColumnName("TempMediaId");

            Property(p => p.UniqName).IsRequired().HasColumnType("varchar").HasMaxLength(255);
            Property(p => p.IsLoading).IsRequired();
            Property(p => p.IsSuccess).IsRequired();

            HasRequired(p => p.User)
                .WithMany(d => d.TempMedia)
                .HasForeignKey(c => c.UserId);
        }
    }
}

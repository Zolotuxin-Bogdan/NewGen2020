using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.DAL.Model.Configuration
{
    public class AttemptLoginConfiguration : EntityTypeConfiguration<Attempt>
    {
        public AttemptLoginConfiguration()
        {
            ToTable("LoginAttempts").HasKey(p => p.Id).Property(p => p.Id).IsRequired().HasColumnName("AttemptId");

            Property(p => p.IpAddress).IsRequired().HasColumnType("varchar").HasMaxLength(30);
            Property(p => p.TimeStamp).IsRequired().HasColumnType("datetime2");
        }
    }
}

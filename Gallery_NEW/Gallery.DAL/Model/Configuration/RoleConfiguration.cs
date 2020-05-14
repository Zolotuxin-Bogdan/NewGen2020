using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.ModelConfiguration;

namespace Gallery.DAL.Model.Configuration
{
    public class RoleConfiguration : EntityTypeConfiguration<Role>
    {
        public RoleConfiguration()
        {
            ToTable("Roles").HasKey(p => p.Id).Property(p => p.Id).IsRequired().HasColumnName("RoleId");

            Property(p => p.Name).IsRequired().HasColumnType("varchar").HasMaxLength(30);
        }
    }
}

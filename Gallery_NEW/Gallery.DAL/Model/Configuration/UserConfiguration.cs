using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.DAL.Model.Configuration
{
    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            ToTable("Users").HasKey(p => p.Id).Property(p => p.Id).IsRequired().HasColumnName("UserId");

            Property(p => p.Email).IsRequired().HasColumnType("varchar").HasMaxLength(30);
            HasIndex(p => p.Email).IsUnique();
            Property(p => p.Password).IsRequired().HasColumnType("varchar").HasMaxLength(30);

            HasMany(p => p.Roles)
                .WithMany(d => d.Users)
                .Map(pd =>
                {
                    pd.MapLeftKey("UserId");
                    pd.MapRightKey("RoleId");
                    pd.ToTable("UsersRoles");
                });

            HasMany(p => p.Media)
                .WithRequired(d => d.User)
                .HasForeignKey(c => c.UserId);

            HasMany(p => p.Attempts)
                .WithRequired(d => d.User)
                .HasForeignKey(c => c.UserId);

            HasMany(p => p.TempMedia)
                .WithRequired(d => d.User)
                .HasForeignKey(c => c.UserId);
        }
    }
}

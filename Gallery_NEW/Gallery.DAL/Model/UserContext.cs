using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.DAL.Model
{
    public class UserContext : DbContext
    {
        public UserContext(string connectionString) : base(connectionString) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Media { get; set; }
        public DbSet<Role> MediaType { get; set; }

    }

    public class UserDbInitializer : DropCreateDatabaseAlways<UserContext>
    {
        protected override void Seed(UserContext ctx)
        {
            User adminUser = new User { Id = 1, Email = "admin@admin.admin", Password = "admin" };

            ctx.Users.AddRange(new List<User> { adminUser });
            ctx.SaveChanges();

            Role role1 = new Role { Name = "admin" };
            Role role2 = new Role { Name = "user" };

            role1.Users.Add(adminUser);

            ctx.Roles.AddRange(new List<Role> { role1, role2});
            ctx.SaveChanges();

        }
    }
}

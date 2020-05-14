using System.Data.Entity;
using Gallery.DAL.Model.Configuration;

namespace Gallery.DAL.Model
{
    public class UserContext : DbContext
    {
        public UserContext() {}
        public UserContext(string connectionString) : base(connectionString) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Media> Media { get; set; }
        public DbSet<MediaType> MediaType { get; set; }
        public DbSet<Attempt> Attempts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.HasDefaultSchema("Gallery");

            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new RoleConfiguration());
            modelBuilder.Configurations.Add(new MediaConfiguration());
            modelBuilder.Configurations.Add(new MediaTypeConfiguration());
            modelBuilder.Configurations.Add(new AttemptLoginConfiguration());

            base.OnModelCreating(modelBuilder);
        }

    }

    
}

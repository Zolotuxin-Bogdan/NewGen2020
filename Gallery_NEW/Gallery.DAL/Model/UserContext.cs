using System.Data.Entity;

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
            // Add Tables to Fluent API
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Role>().ToTable("Roles");
            modelBuilder.Entity<Media>().ToTable("Media");
            modelBuilder.Entity<MediaType>().ToTable("MediaTypes");
            modelBuilder.Entity<Attempt>().ToTable("Attempts");

            // Add Primary Keys to Fluent API
            modelBuilder.Entity<User>().HasKey(p => p.Id);
            modelBuilder.Entity<Role>().HasKey(p => p.Id);
            modelBuilder.Entity<Media>().HasKey(p => p.Id);
            modelBuilder.Entity<MediaType>().HasKey(p => p.Id);
            modelBuilder.Entity<Attempt>().HasKey(p => p.Id);

            // Add Property to Fluent API
            modelBuilder.Entity<User>().Property(p => p.Id).IsRequired();
            modelBuilder.Entity<User>().Property(p => p.Email).IsRequired();
            modelBuilder.Entity<User>().Property(p => p.Password).IsRequired();

            // Add Relations to Fluent API
            modelBuilder.Entity<User>()
                .HasMany(p => p.Roles)
                .WithMany(d => d.Users);

            modelBuilder.Entity<User>()
                .HasMany(p => p.Media)
                .WithRequired(d => d.User);

            modelBuilder.Entity<MediaType>()
                .HasMany(p => p.Media)
                .WithRequired(d => d.Type);

            modelBuilder.Entity<User>()
                .HasMany(p => p.Attempts)
                .WithRequired(d => d.User);

            base.OnModelCreating(modelBuilder);
        }

    }

    
}

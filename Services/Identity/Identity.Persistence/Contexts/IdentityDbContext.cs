using Identity.Domain;
using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Identity.Persistence.Contexts
{
    public class IdentityDbContext : DbContext
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
        {
            if (Database.IsRelational())
            {
                Database.Migrate();
            }
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupUser> GroupUsers { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(IdentityDbContext).Assembly);

            // User
            modelBuilder.Entity<User>()
                .HasIndex(p => p.Email)
                .IsUnique();

            // Group user
            modelBuilder.Entity<GroupUser>()
                .HasKey(p => new { p.GroupId, p.UserId });

            modelBuilder.Entity<GroupUser>()
                .HasOne(p => p.Group)
                .WithMany(g => g.Users)
                .HasForeignKey(gu => gu.GroupId);

            modelBuilder.Entity<GroupUser>()
                .HasOne(p => p.User)
                .WithMany(u => u.Groups)
                .HasForeignKey(gu => gu.UserId);

            base.OnModelCreating(modelBuilder);
        }
    }
}

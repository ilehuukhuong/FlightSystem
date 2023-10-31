using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : IdentityDbContext<AppUser, AppRole, int, IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Configuration> Configurations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            builder.Entity<AppRole>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

            builder.Entity<PermissionUser>()
                .HasKey(k => new { k.AppUserId, k.PermissionId });

            builder.Entity<PermissionUser>()
                .HasOne(s => s.AppUser)
                .WithMany(l => l.PermissionUsers)
                .HasForeignKey(s => s.AppUserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<PermissionUser>()
                .HasOne(s => s.Permission)
                .WithMany(l => l.PermissionUsers)
                .HasForeignKey(s => s.PermissionId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<ConfigurationPermission>()
                .HasKey(k => new { k.PermissionId, k.ConfigurationId });

            builder.Entity<ConfigurationPermission>()
                .HasOne(s => s.Permission)
                .WithMany(l => l.ConfigurationPermissions)
                .HasForeignKey(s => s.PermissionId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<ConfigurationPermission>()
                .HasOne(s => s.Configuration)
                .WithMany(l => l.ConfigurationPermissions)
                .HasForeignKey(s => s.ConfigurationId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PTMS.Domain.Entities;

namespace PTMS.Persistance.EntityConfigurations
{
    public static class AppUserConfiguration
    {
        public static void ConfigureAppUsers(this ModelBuilder builder)
        {
            builder.Entity<AppRole>().ToTable("AppRole");
            builder.Entity<IdentityRoleClaim<int>>().ToTable("AppRoleClaim");
            builder.Entity<IdentityUserClaim<int>>().ToTable("AppUserClaim");
            builder.Entity<IdentityUserLogin<int>>().ToTable("AppUserLogin");
            builder.Entity<IdentityUserToken<int>>().ToTable("AppUserToken");

            builder.Entity<AppUser>(entity =>
            {
                entity.ToTable("AppUser");

                entity
                    .Property(nameof(AppUser.FirstName))
                    .HasMaxLength(15);

                entity
                    .Property(nameof(AppUser.LastName))
                    .HasMaxLength(20);

                entity
                    .Property(nameof(AppUser.MiddleName))
                    .HasMaxLength(20);

                entity
                    .Property(nameof(AppUser.Description))
                    .HasMaxLength(256);

                entity
                    .Property(nameof(AppUser.PasswordHash))
                    .HasMaxLength(500);

                entity
                    .HasMany(e => e.UserRoles)
                    .WithOne(u => u.User)
                    .HasForeignKey(u => u.UserId);
            });

            builder.Entity<AppUserRole>(entity =>
            {
                entity.ToTable("AppUserRole");

                entity.HasKey(ur => new { ur.UserId, ur.RoleId });

                entity.HasOne(ur => ur.Role)
                    .WithMany()
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                entity.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });
        }
    }
}

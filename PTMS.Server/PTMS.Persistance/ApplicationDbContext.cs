using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PTMS.Domain.Entities;

namespace PTMS.Persistance
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, int>
    {
        public DbSet<Transporter> Transporters { get; private set; }
        public DbSet<Vehicle> Vehicles { get; private set; }
        public DbSet<VehicleType> VehicleTypes { get; private set; }
        public DbSet<Route> Routes { get; private set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Remove Pluralizing Table Name Convention
            foreach (IMutableEntityType entity in builder.Model.GetEntityTypes())
            {
                entity.Relational().TableName = entity.DisplayName();
            }
            
            builder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaim");
            builder.Entity<IdentityUserClaim<int>>().ToTable("UserClaim");
            builder.Entity<IdentityUserLogin<int>>().ToTable("UserLogin");
            builder.Entity<IdentityUserRole<int>>().ToTable("UserRole");
            builder.Entity<IdentityUserToken<int>>().ToTable("UserToken");
        }
    }
}

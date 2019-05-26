using Microsoft.EntityFrameworkCore;
using PTMS.Domain.Entities;
using PTMS.Persistance.Converters;

namespace PTMS.Persistance.EntityConfigurations
{
    internal static class RouteConfiguration
    {
        public static void ConfigureRoutes(this ModelBuilder builder)
        {
            builder.Entity<Routs>(entity =>
            {
                entity.ToTable("ROUTS                          ");

                entity.HasIndex(e => e.Id)
                    .HasName("PK_ROUTS");

                entity.Property(e => e.Id).HasColumnName("ID_");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("NAME_")
                    .HasMaxLength(50)
                    .HasAnnotation("Description", "Маршрут");

                entity.Property(e => e.RouteActive)
                    .HasColumnName("ROUTE_ACTIVE_")
                    .HasDefaultValueSql("DEFAULT 1")
                    .HasAnnotation("Description", "Статус активности маршрута")
                    .HasConversion(new IntToBooleanConverter());

                entity.HasMany(e => e.ProjectRoutes)
                    .WithOne(e => e.Route)
                    .HasForeignKey(e => e.RoutId);
            });
        }
    }
}

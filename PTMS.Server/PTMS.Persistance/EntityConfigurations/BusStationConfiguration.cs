using Microsoft.EntityFrameworkCore;
using PTMS.Domain.Entities;

namespace PTMS.Persistance.EntityConfigurations
{
    internal static class BusStationConfiguration
    {
        public static void ConfigureStations(this ModelBuilder builder)
        {
            builder.Entity<BusStation>(entity =>
            {
                entity.ToTable("BS                             ");

                entity.HasIndex(e => e.Id)
                    .HasName("PK_BS_1");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Azimuth).HasColumnName("AZMTH");

                entity.Property(e => e.Latitude).HasColumnName("LAT");

                entity.Property(e => e.Longitude).HasColumnName("LON");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("NAME")
                    .HasMaxLength(110);
            });

            builder.Entity<BusStationRoute>(entity =>
            {
                entity.ToTable("BS_ROUTE                       ");

                entity.HasIndex(e => e.Id)
                    .HasName("PK_BS_ROUTE_1");

                entity.HasIndex(e => new { e.Num, e.RouteId })
                    .HasName("BS_ROUTE_IDX")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BusStationId).HasColumnName("BS_ID");

                entity.Property(e => e.Num).HasColumnName("NUM");

                entity.Property(e => e.RouteId).HasColumnName("ROUTE_ID");

                entity.Property(e => e.IsEndingStation)
                    .HasColumnName("Control")
                    .HasDefaultValue(false);

                entity.HasOne(e => e.BusStation)
                    .WithMany()
                    .HasForeignKey(e => e.BusStationId);
            });
        }
    }
}

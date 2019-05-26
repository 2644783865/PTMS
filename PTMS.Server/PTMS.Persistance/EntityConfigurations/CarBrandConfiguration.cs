using Microsoft.EntityFrameworkCore;
using PTMS.Domain.Entities;

namespace PTMS.Persistance.EntityConfigurations
{
    public static class CarBrandConfiguration
    {
        public static void ConfigureCarBrands(this ModelBuilder builder)
        {
            builder.Entity<CarBrand>(entity =>
            {
                entity.HasKey(e => e.CbId)
                    .HasName("PK_CAR_BRAND");

                entity.ToTable("CAR_BRAND                      ");

                entity.HasIndex(e => e.CarTypeId)
                    .HasName("FK_CAR_BRAND_CAR_TYPE")
                    .IsUnique();

                entity.HasIndex(e => e.CbId)
                    .HasName("PK_CAR_BRAND");

                entity.HasIndex(e => e.CbName)
                    .HasName("UNQ1_CAR_BRAND");

                entity.Property(e => e.CbId).HasColumnName("CB_ID_");

                entity.Property(e => e.CarTypeId)
                    .HasColumnName("CAR_TYPE_ID_")
                    .HasAnnotation("Description", "Тип автобуса");

                entity.Property(e => e.CbName)
                    .IsRequired()
                    .HasColumnName("CB_NAME_")
                    .HasMaxLength(50)
                    .HasAnnotation("Description", "Марка автобуса");

                entity.Property(e => e.H).HasMaxLength(10);

                entity.Property(e => e.L).HasMaxLength(10);

                entity.Property(e => e.W).HasMaxLength(10);

                entity.HasOne(e => e.CarType)
                    .WithMany()
                    .HasForeignKey(e => e.CarTypeId);
            });
        }
    }
}

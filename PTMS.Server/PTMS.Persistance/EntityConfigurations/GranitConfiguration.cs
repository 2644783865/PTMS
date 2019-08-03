using Microsoft.EntityFrameworkCore;
using PTMS.Domain.Entities;

namespace PTMS.Persistance.EntityConfigurations
{
    public static class GranitConfiguration
    {
        public static void ConfigureGranits(this ModelBuilder builder)
        {
            builder.Entity<Granit>(entity =>
            {
                entity.ToTable("GRANITS                        ");

                entity.HasIndex(e => e.BlockNumber)
                    .HasName("UNQ1_BLOCK_NUM");

                entity.HasIndex(e => e.BlockTypeId)
                    .HasName("FK_GRANITS_2")
                    .IsUnique();

                entity.HasIndex(e => e.Id)
                    .HasName("PK_GRANITS");

                entity.HasIndex(e => e.ObjectId)
                    .HasName("GRANITS_IDX1");

                entity.Property(e => e.Id).HasColumnName("ID_");

                entity.Property(e => e.BlockNumber)
                    .HasColumnName("BLOCK_NUMBER")
                    .HasAnnotation("Description", "Номер блока");

                entity.Property(e => e.BlockTypeId)
                    .HasColumnName("BLOCK_TYPE")
                    .HasAnnotation("Description", "Тип блока");

                entity.Property(e => e.ObjectId)
                    .HasColumnName("OIDS_")
                    .HasAnnotation("Description", "ID автобуса");

                entity.HasOne(x => x.BlockType)
                    .WithMany()
                    .HasForeignKey(x => x.BlockTypeId);
            });
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PTMS.Domain.Entities;
using PTMS.Domain.Enums;

namespace PTMS.Persistance.EntityConfigurations
{
    internal static class EventLogConfiguration
    {
        public static void ConfigureEventLogs(this ModelBuilder builder)
        {
            builder.Entity<EventLog>(entity =>
            {
                entity.ToTable("EventLog");

                entity.HasKey(e => e.Id);

                entity.HasIndex(e => e.TimeStamp);

                entity.Property(e => e.Event)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasConversion(new EnumToStringConverter<EventEnum>());

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.EntityType)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .HasConstraintName("FK_UserId");

                entity.HasMany(e => e.EventLogFields)
                    .WithOne(e => e.EventLog)
                    .HasForeignKey(e => e.EventLogId)
                    .HasConstraintName("FK_EventLogId");
            });

            builder.Entity<EventLogField>(entity =>
            {
                entity.ToTable("EventLogField");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.FieldName)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.OldFieldValue)
                    .HasMaxLength(150);

                entity.Property(e => e.NewFieldValue)
                    .HasMaxLength(150);
            });
        }
    }
}

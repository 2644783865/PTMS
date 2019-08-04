using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using PTMS.Domain.Entities;
using PTMS.Persistance.Converters;

namespace PTMS.Persistance.EntityConfigurations
{
    internal static class ObjectConfiguration
    {
        public static void ConfigureObjects(this ModelBuilder builder)
        {
            builder.Entity<Objects>(entity =>
            {
                //entity.HasKey(e => new { e.ProjId, e.ObjId })
                //    .HasName("PK_OBJECTS");

                entity.HasKey(e => e.Id)
                    .HasName("PK_OBJECTS");

                entity.ToTable("OBJECTS                        ");

                entity.HasIndex(e => e.CarBrandId)
                    .HasName("FK_OBJECTS_1")
                    .IsUnique();

                entity.HasIndex(e => e.Id)
                    .HasName("UNQ_IDS");

                entity.HasIndex(e => e.LastRout)
                    .HasName("IDX_LAST_ROUT")
                    .IsUnique();

                entity.HasIndex(e => e.Name)
                    .HasName("UNQ1_OBJECTS");

                entity.HasIndex(e => e.ObjOutput)
                    .HasName("IDX_OBJ_OUTPUT")
                    .IsUnique();

                entity.HasIndex(e => e.Phone)
                    .HasName("UNQ_OBJECTS_PHONE");

                entity.HasIndex(e => e.ProviderId)
                    .HasName("FK_OBJECTS_3")
                    .IsUnique();

                entity.HasIndex(e => e.CarTypeId)
                    .HasName("FK_OBJECTS_2")
                    .IsUnique();

                entity.HasIndex(e => new { e.ObjId, e.ProjId })
                    .HasName("PK_OBJECTS");

                entity.Property(e => e.ProjId)
                    .HasColumnName("PROJ_ID_")
                    .HasAnnotation("Description", "Перевозчик");

                entity.Property(e => e.ObjId)
                    .HasColumnName("OBJ_ID_")
                    .HasAnnotation("Description", "Объект");

                entity.Property(e => e.Azmth)
                    .HasColumnName("AZMTH_")
                    .HasDefaultValueSql("DEFAULT 0");

                entity.Property(e => e.CarBrandId)
                    .HasColumnName("CAR_BRAND_")
                    .HasDefaultValueSql("DEFAULT 0")
                    .HasAnnotation("Description", "Марка");

                entity.Property(e => e.DateInserted)
                    .HasColumnName("DATE_INSERTED_")
                    .HasAnnotation("Description", "Дата ввода");

                entity.Property(e => e.DispRoute)
                    .HasColumnName("DISP_ROUTE_")
                    .HasAnnotation("Description", "Транслятор");

                var converter = new ValueConverter<int, decimal>(
                    v => v,
                    v => (int)v,
                    new ConverterMappingHints(valueGeneratorFactory: (p, t) => new TemporaryIntValueGenerator()));

                entity.Property(e => e.Id)
                    .HasColumnName("IDS_")
                    .HasColumnType("NUMERIC(9, 0)")
                    .HasConversion(converter);

                entity.Property(e => e.LastAddInfo)
                    .HasColumnName("LAST_ADD_INFO_");

                entity.Property(e => e.LastLat)
                    .HasColumnName("LAST_LAT_")
                    .HasDefaultValueSql("DEFAULT 0")
                    .HasAnnotation("Description", "Долгота");

                entity.Property(e => e.LastLon)
                    .HasColumnName("LAST_LON_")
                    .HasDefaultValueSql("DEFAULT 0")
                    .HasAnnotation("Description", "Широта");

                entity.Property(e => e.LastRout)
                    .HasColumnName("LAST_ROUT_")
                    .HasAnnotation("Description", "Последний маршрут");

                entity.Property(e => e.LastSpeed)
                    .HasColumnName("LAST_SPEED_")
                    .HasDefaultValueSql("DEFAULT 0")
                    .HasAnnotation("Description", "Последняя скорость");

                entity.Property(e => e.LastStation)
                    .HasColumnName("LAST_STATION_")
                    .HasAnnotation("Description", "Последняя остановка");

                entity.Property(e => e.LastStationTime)
                    .HasColumnName("LAST_STATION_TIME_")
                    .HasAnnotation("Description", "Время последней остановки");

                entity.Property(e => e.LastTime)
                    .HasColumnName("LAST_TIME_")
                    .HasDefaultValueSql("default CURRENT_TIMESTAMP")
                    .HasAnnotation("Description", "Время последнего отклика");

                entity.Property(e => e.Lowfloor)
                    .HasColumnName("LOWFLOOR");

                entity.Property(e => e.Name)
                    .HasColumnName("NAME_")
                    .HasMaxLength(20)
                    .HasAnnotation("Description", "Гос.номер");

                entity.Property(e => e.ObjOutput)
                    .HasColumnName("OBJ_OUTPUT_")
                    .HasDefaultValueSql("DEFAULT 1")
                    .HasAnnotation("Description", "Статус вывода")
                    .HasConversion(new IntToBooleanConverter());

                entity.Property(e => e.ObjOutputDate)
                    .HasColumnName("OBJ_OUTPUT_DATE_")
                    .HasAnnotation("Description", "Время вывода");

                entity.Property(e => e.Phone)
                    .HasColumnName("PHONE_")
                    .HasAnnotation("Description", "Телефон");

                entity.Property(e => e.ProviderId)
                    .HasColumnName("PROVIDER_")
                    .HasDefaultValueSql("DEFAULT 0")
                    .HasAnnotation("Description", "Установщик");

                entity.Property(e => e.UserComment)
                    .HasColumnName("USER_COMMENT_")
                    .HasMaxLength(100);

                entity.Property(e => e.CarTypeId)
                    .HasColumnName("VEHICLE_TYPE_")
                    .HasDefaultValueSql("DEFAULT 1")
                    .HasAnnotation("Description", "Тип автобуса");

                entity.Property(e => e.YearRelease)
                    .HasColumnName("YEAR_RELEASE_")
                    .HasAnnotation("Description", "Год выпуска");

                entity.HasOne(e => e.Provider)
                    .WithMany(e => e.Objects)
                    .HasForeignKey(e => e.ProviderId);

                entity.HasOne(e => e.CarBrand)
                    .WithMany(e => e.Objects)
                    .HasForeignKey(e => e.CarBrandId);

                entity.HasOne(e => e.Project)
                    .WithMany()
                    .HasForeignKey(e => e.ProjId);

                entity.HasOne(e => e.Route)
                    .WithMany()
                    .HasForeignKey(e => e.LastRout);

                entity.HasOne(e => e.Block)
                    .WithOne(e => e.Object)
                    .HasForeignKey<Granit>(e => e.ObjectId);
            });
        }
    }
}

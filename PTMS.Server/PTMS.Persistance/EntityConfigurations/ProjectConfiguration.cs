using Microsoft.EntityFrameworkCore;
using PTMS.Domain.Entities;

namespace PTMS.Persistance.EntityConfigurations
{
    public static class ProjectConfiguration
    {
        public static void ConfigureProjects(this ModelBuilder builder)
        {
            builder.Entity<Project>(entity =>
            {
                entity.ToTable("PROJECTS                       ");

                entity.HasIndex(e => e.Id)
                    .HasName("PK_PROJECTS");

                entity.HasIndex(e => e.Name)
                    .HasName("UNQ1_PROJECTS");

                entity.Property(e => e.Id).HasColumnName("ID_");

                entity.Property(e => e.Director)
                    .HasColumnName("DIRECTOR")
                    .HasMaxLength(50)
                    .HasAnnotation("Description", "Директор");

                entity.Property(e => e.Name)
                    .HasColumnName("NAME_")
                    .HasMaxLength(50)
                    .HasAnnotation("Description", "Перевозчик");

                entity.HasMany(e => e.ProjectRoutes)
                    .WithOne(e => e.Project)
                    .HasForeignKey(e => e.ProjId);
            });
        }
    }
}

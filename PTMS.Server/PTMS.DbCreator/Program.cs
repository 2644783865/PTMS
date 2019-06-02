using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using PTMS.Persistance;

namespace PTMS.DbCreator
{
    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
                
                var builder = new ConfigurationBuilder()
                  .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true)
                  .AddEnvironmentVariables();

                IConfigurationRoot configuration = builder.Build();

                var connectionString = configuration.GetConnectionString("DefaultConnection");

                var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                optionsBuilder.UseFirebird(connectionString);

                var context = new ApplicationDbContext(optionsBuilder.Options);

                var ifDbExist = (context.Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists();

                //Create db
                context.Database.Migrate();

                Console.WriteLine("Migrations have been successfully applied");

                return 0;
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);

#if DEBUG
                Console.ReadLine();
#endif

                return -1;
            }
        }
    }
}

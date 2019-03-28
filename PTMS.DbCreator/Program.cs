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
        static void Main(string[] args)
        {
            try
            {
                var builder = new ConfigurationBuilder()
                  .SetBasePath(Directory.GetCurrentDirectory())
                  .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

                IConfigurationRoot configuration = builder.Build();

                var connectionString = configuration.GetConnectionString("DefaultConnection");

                var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                optionsBuilder.UseSqlServer(connectionString);

                var context = new ApplicationDbContext(optionsBuilder.Options);

                var ifDbExist = (context.Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists();

                //Create db
                context.Database.Migrate();

                //IF db didn't exist - add test data
                if (!ifDbExist)
                {
                    PtmsDbInitializer.Initialize(context);
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
                Console.ReadLine();
            }
        }
    }
}

﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using PTMS.Common;
using PTMS.Persistance;
using System;

namespace PTMS.DbCreator
{
    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                var builder = new ConfigurationBuilder()
                  .AddJsonFile($"appsettings.json", optional: true, reloadOnChange: true)
                  .AddEnvironmentVariables();

                IConfigurationRoot configuration = builder.Build();

                var connectionString = configuration.GetSection("AppSettings").Get<AppSettings>().ProjectsDatabaseConnection;

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

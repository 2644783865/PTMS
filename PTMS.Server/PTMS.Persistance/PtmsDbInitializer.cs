using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PTMS.Common;
using PTMS.Domain.Entities;
using VehicleType = PTMS.Domain.Enums.VehicleType;

namespace PTMS.Persistance
{
    public static class PtmsDbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            CreateVehicleTypes(context);
            CreateRoutes(context);
            CreateRoles(context);
            CreateTransportersWithVehicles(context);
            CreateUsers(context);
        }

        private static void CreateRoles(ApplicationDbContext context)
        {
            var roles = new Role[]
            {
                new Role
                {
                    Name = RoleNames.Administrator,
                    DisplayName = "Администратор"
                },
                new Role
                {
                    Name = RoleNames.Transporter,
                    DisplayName = "Перевозчик"
                },
                new Role
                {
                    Name = RoleNames.Dispatcher,
                    DisplayName = "Диспетчер"
                },
            };

            foreach (var role in roles)
            {
                role.NormalizedName = role.Name.ToUpper();

                context.Roles.Add(role);
            }

            context.SaveChanges();
        }

        private static void CreateUsers(ApplicationDbContext context)
        {
            //password = test
            var password = "AQAAAAEAACcQAAAAECQQ5jCvhsKCrCKH5WmHU3gnzEYpV45hxmYy/jjcFBgPs32cDu/Z4BPONUA3vjFxqA==";

            //Add users
            var users = new User[]
            {
                new User
                {
                    LastName = "Иванов",
                    FirstName = "Иван",
                    MiddleName = "Иванович",
                    Email = "admin@test.com",
                    Description = "Администратор ЦОДД",
                    PhoneNumber = "+79511111111"
                },
                new User
                {
                    LastName = "Петров",
                    FirstName = "Пётр",
                    MiddleName = "Петрович",
                    Email = "transporter@test.com",
                    Description = "Директор транспортной компании",
                    PhoneNumber = "+79522222222",
                    TransporterId = 1
                },
                new User
                {
                    LastName = "Иванов",
                    FirstName = "Иван",
                    MiddleName = "Иванович",
                    Email = "dispatcher@test.com",
                    Description = "Диспетчер на участке 1",
                    PhoneNumber = "+79513333333"
                }
            };

            foreach (var user in users)
            {
                user.UserName = user.Email;
                user.NormalizedUserName = user.Email.ToUpper();
                user.NormalizedEmail = user.Email.ToUpper();
                user.EmailConfirmed = true;
                user.PasswordHash = password;
                user.AccessFailedCount = 0;
                user.ConcurrencyStamp = Guid.NewGuid().ToString();
                user.LockoutEnabled = false;
                user.PhoneNumberConfirmed = true;
                user.SecurityStamp = Guid.NewGuid().ToString();
                user.TwoFactorEnabled = false;

                context.Users.Add(user);
            }

            context.SaveChanges();

            //Add user roles
            var userRoles = new IdentityUserRole<int>[]
            {
                new IdentityUserRole<int>
                {
                    UserId = 1,
                    RoleId = 1
                },
                new IdentityUserRole<int>
                {
                    UserId = 2,
                    RoleId = 2
                },
                new IdentityUserRole<int>
                {
                    UserId = 3,
                    RoleId = 3
                }
            };

            foreach (var userRole in userRoles)
            {
                context.UserRoles.Add(userRole);
            }

            context.SaveChanges();
        }

        private static void CreateTransportersWithVehicles(ApplicationDbContext context)
        {
            var routes = context.Routes.ToList();

            var data = new[]
            {
                new {
                    TransporterName = "АвтоЛайн+",
                    Vehicles = new Dictionary<string, Vehicle>()
                    {
                        {
                            "5А",
                            new Vehicle
                            {
                                PlateNumber = "test1",
                                VehicleTypeId = (int)VehicleType.Bus
                            }
                        },
                        {
                            "11",
                            new Vehicle
                            {
                                PlateNumber = "test2",
                                VehicleTypeId = (int)VehicleType.Bus
                            }
                        },
                        {
                            "49М",
                            new Vehicle
                            {
                                PlateNumber = "test3",
                                VehicleTypeId = (int)VehicleType.ShuttleBus
                            }
                        }
                    }
                },
                new {
                    TransporterName = "ООО \"Автоуслуги-Н\"",
                    Vehicles = new Dictionary<string, Vehicle>()
                    {
                        {
                            "120В",
                            new Vehicle
                            {
                                PlateNumber = "test4",
                                VehicleTypeId = (int)VehicleType.Bus
                            }
                        },
                        {
                            "66",
                            new Vehicle
                            {
                                PlateNumber = "test5",
                                VehicleTypeId = (int)VehicleType.Bus
                            }
                        }
                    }
                }
            };
            
            foreach (var item in data)
            {
                var transporter = new Transporter()
                {
                    Name = item.TransporterName,
                    Vehicles = item.Vehicles.Select(x =>
                    {
                        var routeId = routes.First(y => y.Name == x.Key).Id;

                        var vehicle = x.Value;
                        vehicle.RouteId = routeId;

                        return vehicle;
                    })
                    .ToList()
                };

                context.Transporters.Add(transporter);
            }

            context.SaveChanges();
        }

        private static void CreateVehicleTypes(ApplicationDbContext context)
        {
            context.Database.OpenConnection();

            context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.[VehicleType] ON");

            foreach (VehicleType type in Enum.GetValues(typeof(VehicleType)))
            {
                var vehicleType = new Domain.Entities.VehicleType
                {
                    Id = (int)type,
                    Name = EnumHelper.GetDescription(type)
                };

                context.Add(vehicleType);
            }

            context.SaveChanges();
            context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.[VehicleType] OFF");

            context.Database.CloseConnection();
        }

        private static void CreateRoutes(ApplicationDbContext context)
        {
            var routes = new Route[]
            {
                new Route
                {
                    Name = "5А",
                },
                new Route
                {
                    Name = "11"
                },
                new Route
                {
                    Name = "25А"
                },
                new Route
                {
                    Name = "34"
                },
                new Route
                {
                    Name = "37А"
                },
                new Route
                {
                    Name = "49Б"
                },
                new Route
                {
                    Name = "49М"
                },
                new Route
                {
                    Name = "125"
                },
                new Route
                {
                    Name = "120В"
                },
                new Route
                {
                    Name = "66"
                },
                new Route
                {
                    Name = "115"
                },
            };

            foreach (var route in routes)
            {
                context.Routes.Add(route);
            }

            context.SaveChanges();
        }
    }
}

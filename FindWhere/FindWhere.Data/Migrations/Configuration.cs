namespace FindWhere.Data.Migrations
{
    using Common;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Model;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<FindWhereDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(FindWhereDbContext context)
        {
            // The Seed method will be called after migrating to the latest version.

            // TODO: Seed data for all models.
            this.SeedUsers(context);

            this.SeedRoles(context);    // Shouled be called after seeding the users!!!

            this.SeedCountries(context);
            this.SeedCities(context);
            this.SeedNeighbourhoods(context);
            this.SeedCategories(context);
            this.SeedLocations(context);
        }

        private void SeedCountries(FindWhereDbContext context)
        {
            if (context.Countries.Any())
            {
                return;
            }

            context.Countries.Add(new Country { Name = "България" });

            context.SaveChanges();
        }

        private void SeedCities(FindWhereDbContext context)
        {
            if (context.Cities.Any())
            {
                return;
            }

            var bulgaria = context.Countries.FirstOrDefault(c => c.Name == "България");
            var sofia = new City { Name = "София" };

            if (bulgaria != null)
            {
                bulgaria.Cities.Add(sofia);
            }

            context.SaveChanges();
        }

        private void SeedNeighbourhoods(FindWhereDbContext context)
        {
            if (context.Neighbourhoods.Any())
            {
                return;
            }

            var neighbourhoods = new List<Neighbourhood>
            {
                new Neighbourhood { Name = "7-ми 11-ти километър" },
                new Neighbourhood { Name = "ж.к. Студентски град" },
                new Neighbourhood { Name = "ж.к. Дианабад" }
            };

            var sofia = context.Cities.FirstOrDefault(c => c.Name == "София");
            if (sofia != null)
            {
                foreach (var item in neighbourhoods)
                {
                    sofia.Neighbourhoods.Add(item);
                }

            }

            context.SaveChanges();
        }

        private void SeedCategories(FindWhereDbContext context)
        {
            if (context.Categories.Any())
            {
                return;
            }

            var categories = new List<Category>
            {
                new Category { Name = "Other" },
                new Category { Name = "Supermarket" },
                new Category { Name = "Pharmacy" },
                new Category { Name = "Ironware" },
                new Category { Name = "Household Goods" },
                new Category { Name = "Grocery Store" },
                new Category { Name = "Restaurant" },
                new Category { Name = "Fast food" }
            };

            foreach (var category in categories)
            {
                context.Categories.Add(category);
            }

            context.SaveChanges();
        }

        private void SeedLocations(FindWhereDbContext context)
        {
            if (context.Locations.Any())
            {
                return;
            }

            var admin = context.Users.FirstOrDefault(u => u.Email == "admin@admin.bg");
            var studentCity = context.Neighbourhoods.FirstOrDefault(n => n.Name == "ж.к. Студентски град");
            var supermarket = context.Categories.FirstOrDefault(c => c.Name == "Supermarket");
            var householdGoods = context.Categories.FirstOrDefault(c => c.Name == "Household Goods");

            if (admin != null && studentCity != null && supermarket != null && householdGoods != null)
            {
                var locations = new List<Location>
                {
                    new Location
                    {
                        FullAddress = "улица „Антон Найденов“ 12, 1700 София, България",
                        IsApproved = true,
                        Latitude =  42.65000430000001,
                        Longitude = 23.341308099999992,
                        Neighbourhood = studentCity,
                        PlaceId = "ChIJAYbEYT2EqkARgwXjrjbxAGI",
                        ShoppingCenter = new ShoppingCenter
                            {
                                Category = supermarket,
                                Details = "Very large supermarket. A lot of groceries. Nice staff.",
                                WorkTime = "00:00 - 24:00"
                            },
                        User = admin
                    },
                    new Location
                    {
                        FullAddress = "улица „акад. Борис Стефанов\" 14, 1700 София, България",
                        IsApproved = true,
                        Latitude =  42.6483173,
                        Longitude = 23.341426700000056,
                        Neighbourhood = studentCity,
                        PlaceId = "ChIJzbcojxeEqkARYzcWSVJqs8k",
                        ShoppingCenter = new ShoppingCenter
                            {
                                Category = householdGoods,
                                Details = "Household goods at low price.",
                                WorkTime = "09:00 - 18:00"
                            },
                        User = admin
                    }
                };

                foreach (var location in locations)
                {
                    context.Locations.Add(location);
                }
            }

            context.SaveChanges();
        }

        private void SeedUsers(FindWhereDbContext context)
        {
            if (context.Users.Any())
            {
                return;
            }

            var admin = new User
            {
                UserName = "admin@admin.bg",
                Email = "admin@admin.bg",
                PasswordHash = new PasswordHasher().HashPassword("admin"),
                LockoutEnabled = true,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var moderator = new User
            {
                UserName = "moderator@moderator.bg",
                Email = "moderator@moderator.bg",
                PasswordHash = new PasswordHasher().HashPassword("moderator"),
                LockoutEnabled = true,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var user = new User
            {
                UserName = "user@user.bg",
                Email = "user@user.bg",
                PasswordHash = new PasswordHasher().HashPassword("user"),
                LockoutEnabled = true,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            context.Users.Add(admin);
            context.Users.Add(moderator);
            context.Users.Add(user);

            context.SaveChanges();
        }

        private void SeedRoles(FindWhereDbContext context)
        {
            if (context.Roles.Any())
            {
                return;
            }

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<User>(new UserStore<User>(context));

            roleManager.Create(new IdentityRole(UserRoles.Admin));
            roleManager.Create(new IdentityRole(UserRoles.Moderator));
            roleManager.Create(new IdentityRole(UserRoles.User));
            context.SaveChanges();

            var admin = context.Users.FirstOrDefault(u => u.Email == "admin@admin.bg");
            if (admin != null)
            {
                userManager.AddToRole(admin.Id, UserRoles.Admin);
                userManager.AddToRole(admin.Id, UserRoles.Moderator);
            }

            var moderator = context.Users.FirstOrDefault(u => u.Email == "moderator@moderator.bg");
            if (moderator != null)
            {
                userManager.AddToRole(moderator.Id, UserRoles.Moderator);
            }

            var allUsers = context.Users.ToList();
            foreach (var user in allUsers)
            {
                userManager.AddToRole(user.Id, UserRoles.User);
            }

            context.SaveChanges();
        }
    }
}

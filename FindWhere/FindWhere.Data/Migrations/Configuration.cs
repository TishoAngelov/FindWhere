namespace FindWhere.Data.Migrations
{
    using Common;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Model;
    using System;
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
            //  This method will be called after migrating to the latest version.

            // TODO: Seed data for all models.
            this.SeedUsers(context);


            this.SeedCountries(context);
            //this.SeedCities(context);
            //this.SeedLocations(context);
            //this.SeedNeighbourhoods(context);


            this.SeedRoles(context);    // Shouled be called after seeding the users!!!
        }

        private void SeedRoles(FindWhereDbContext context)
        {
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

        private void SeedUsers(FindWhereDbContext context)
        {
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

            context.Users.AddOrUpdate(
                u => u.Email,
                admin,
                moderator,
                user
            );

            context.SaveChanges();
        }

        private void SeedNeighbourhoods(FindWhereDbContext context)
        {
            throw new NotImplementedException();

            context.SaveChanges();
        }

        private void SeedLocations(FindWhereDbContext context)
        {
            throw new NotImplementedException();

            context.SaveChanges();
        }

        private void SeedCities(FindWhereDbContext context)
        {
            throw new NotImplementedException();

            context.SaveChanges();
        }

        private void SeedCountries(FindWhereDbContext context)
        {
            context.Countries.AddOrUpdate(
                  c => c.Name,
                  new Country { Name = "България" }
            );

            context.SaveChanges();
        }
    }
}

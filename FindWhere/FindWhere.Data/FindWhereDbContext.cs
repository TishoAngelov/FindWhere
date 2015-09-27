namespace FindWhere.Data
{
    using Migrations;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Model;
    using System.Data.Entity;
    using Contracts.CodeFirstConventions;

    public class FindWhereDbContext : IdentityDbContext<User>
    {
        // AppharborConnection - use it before commit
        // DefaultConnection
        public FindWhereDbContext() : base("AppharborConnection")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<FindWhereDbContext, Configuration>());
        }

        public static FindWhereDbContext Create()
        {
            return new FindWhereDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Conventions.Add(new IsUnicodeAttributeConvention());   // Add this so you can add unicode to database.
            modelBuilder.Properties<string>().Configure(p => p.IsUnicode(true));
            base.OnModelCreating(modelBuilder);     // Without this call EntityFramework won't be able to configure the identity model.
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public virtual IDbSet<Country> Countries { get; set; }

        public virtual IDbSet<City> Cities { get; set; }

        public virtual IDbSet<Neighbourhood> Neighbourhoods { get; set; }

        public virtual IDbSet<Location> Locations { get; set; }

        public virtual IDbSet<Category> Categories { get; set; }

        public virtual IDbSet<ShoppingCenter> ShoppingCenters { get; set; }
    }
}
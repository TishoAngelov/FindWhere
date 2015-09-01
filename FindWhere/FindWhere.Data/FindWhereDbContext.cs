namespace FindWhere.Data
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using Model;
    using System.Data.Entity;

    public class FindWhereDbContext : IdentityDbContext<User>
    {
        public FindWhereDbContext() : base("DefaultConnection")
        {
        }

        public static FindWhereDbContext Create()
        {
            return new FindWhereDbContext();
        }

        // TODO: Add all models
        public virtual IDbSet<Country> Countries { get; set; }
    }
}
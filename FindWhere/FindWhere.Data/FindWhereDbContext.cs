namespace FindWhere.Data
{
    using Model;
    using System.Data.Entity;

    public class FindWhereDbContext : DbContext
    {
        public FindWhereDbContext() : base("DefaultConnection")
        {
        }

        // TODO: Add all models
        public virtual IDbSet<Country> Countries { get; set; }
    }
}
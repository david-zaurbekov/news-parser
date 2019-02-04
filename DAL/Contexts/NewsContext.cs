using Data;
using System.Data.Entity;

namespace DAL.Contexts
{
    public class NewsContext : DbContext
    {
        public NewsContext() : base("DefaultConnection")
        {
            Database.SetInitializer(new NewsDBInitializer());
        }

        public DbSet<Article> Articles { get; set; }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }
}

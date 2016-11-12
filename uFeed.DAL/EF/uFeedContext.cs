using System.Data.Entity;
using uFeed.DAL.Entities;

namespace uFeed.DAL.EF
{
    public class UFeedContext : DbContext
    {
        static UFeedContext()
        {
            Database.SetInitializer(new StoreDbInitializer());
        }

        public UFeedContext(string connectionString)
            : base(connectionString)
        {
        }

        public virtual DbSet<ClientProfile> ClientProfiles { get; set; }

        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<SocialAuthor> SocialAuthors { get; set; }
    }
}

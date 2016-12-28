using System.Data.Entity;
using uFeed.Entities;

namespace uFeed.DAL.EF
{
    public class UFeedContext : DbContext
    {
        static UFeedContext()
        {
            Database.SetInitializer(new UFeedDbInitializer());
        }

        public UFeedContext(string connectionString)
            : base(connectionString)
        {
        }
  
        public virtual DbSet<User> ClientProfiles { get; set; }

        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<SocialAuthor> SocialAuthors { get; set; }

        public virtual DbSet<ClientBookmark> ClientBookmarks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
            .HasMany(a => a.Categories)
            .WithRequired(a => a.User)
            .WillCascadeOnDelete(true);

            modelBuilder.Entity<Category>()
            .HasMany(a => a.Authors)
            .WithRequired(a => a.Category)
            .WillCascadeOnDelete(true);
        }
    }
}

using System.Data.Entity;
using uFeed.Entities;

namespace uFeed.DAL.EF
{
    public class UFeedDbInitializer : DropCreateDatabaseAlways<UFeedContext>
    {
        protected override void Seed(UFeedContext db)
        {
            var clientProfile = new User
            {
                Id = 1,
                Email = "qwe@qwe",
                Name = "qwe",
                PasswordHash = "11111"
            };

            db.ClientProfiles.Add(clientProfile);

            db.SaveChanges();
        }
    }
}
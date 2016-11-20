using System.Data.Entity;
using uFeed.Entities;

namespace uFeed.DAL.EF
{
    public class UFeedDbInitializer : DropCreateDatabaseAlways<UFeedContext>
    {
        protected override void Seed(UFeedContext db)
        {
            var clientProfile = new ClientProfile
            {
                Id = 1,
                UserId = 1,
                Email = "qwe@qwe",
                Name = "qwe"
            };

            db.ClientProfiles.Add(clientProfile);

            db.SaveChanges();
        }
    }
}
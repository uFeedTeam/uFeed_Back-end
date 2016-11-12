using System.Data.Entity;

namespace uFeed.DAL.EF
{
    public class UFeedDbInitializer : DropCreateDatabaseAlways<UFeedContext>
    {
        protected override void Seed(UFeedContext db)
        {           
            db.SaveChanges();
        }
    }
}
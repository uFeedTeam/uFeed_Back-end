using System.Data.Entity;

namespace uFeed.DAL.EF
{
    public class StoreDbInitializer : DropCreateDatabaseAlways<UFeedContext>
    {
        protected override void Seed(UFeedContext db)
        {           
            db.SaveChanges();
        }
    }
}
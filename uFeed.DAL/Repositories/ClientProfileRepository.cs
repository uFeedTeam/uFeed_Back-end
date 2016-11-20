using System.Data.Entity;
using System.Linq;
using uFeed.Entities;

namespace uFeed.DAL.Repositories
{
    public class ClientProfileRepository : CommonRepository<ClientProfile>
    {
        public ClientProfileRepository(DbContext context) : base(context)
        {
        }

        public ClientProfile GetByUserId(int userId)
        {
            return Find(profile => profile.UserId == userId).FirstOrDefault();
        }
    }
}

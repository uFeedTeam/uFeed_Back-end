using System.Collections.Generic;
using uFeed.DAL.Entities.Social;

namespace uFeed.DAL.Entities.Attach
{
    public class Album : List<Photo>
    {
        public string Description { get; set; }
    }
}
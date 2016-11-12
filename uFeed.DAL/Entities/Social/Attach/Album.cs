using System.Collections.Generic;

namespace uFeed.DAL.Entities.Social.Attach
{
    public class Album : List<Photo>
    {
        public string Description { get; set; }
    }
}
using System.Collections.Generic;

namespace uFeed.Entities.Social.Attach
{
    public class Album : List<Photo>
    {
        public string Description { get; set; }
    }
}
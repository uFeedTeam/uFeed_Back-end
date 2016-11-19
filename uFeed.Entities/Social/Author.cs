using System.Collections.Generic;
using uFeed.Entities.Enums;

namespace uFeed.Entities.Social
{
    public class Author
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public Socials Source { get; set; }

        public Photo Photo { get; set; }

        public virtual ICollection<Category> Categories { get; set; }

    }
}
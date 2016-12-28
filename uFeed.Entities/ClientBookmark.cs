using uFeed.Entities.Enums;

namespace uFeed.Entities
{
    public class ClientBookmark : BaseType
    {
        public virtual User ClientProfile { get; set; }

        public string PostId { get; set; }

        public Socials Source { get; set; }
    }
}
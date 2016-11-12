using System.Collections.Generic;

namespace uFeed.BLL.DTO
{
    public class CategoryDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ClientProfileDTO User { get; set; }

        public virtual ICollection<SocialAuthorDTO> Authors { get; set; }
    }
}

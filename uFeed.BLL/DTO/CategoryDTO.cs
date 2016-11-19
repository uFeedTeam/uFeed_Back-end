using System.Collections.Generic;

namespace uFeed.BLL.DTO
{
    public class CategoryDTO
    {
        public CategoryDTO()
        {
            Authors = new List<SocialAuthorDTO>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int UserId { get; set; }

        public ClientProfileDTO User { get; set; }

        public ICollection<SocialAuthorDTO> Authors { get; set; }
    }
}

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

        //public ClientProfileDTO User { get; set; }//Commented because of self loop in category

        public int ClientProfileId { get; set; }

        public ICollection<SocialAuthorDTO> Authors { get; set; }
    }
}

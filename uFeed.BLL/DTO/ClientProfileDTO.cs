using System.Collections.Generic;

namespace uFeed.BLL.DTO
{
    public class ClientProfileDTO
    {
        public ClientProfileDTO()
        {
            Categories = new List<CategoryDTO>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public ICollection<CategoryDTO> Categories { get; set; }
    }
}

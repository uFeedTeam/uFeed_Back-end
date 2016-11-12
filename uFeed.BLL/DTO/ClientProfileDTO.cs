using System.Collections.Generic;

namespace uFeed.BLL.DTO
{
    public class ClientProfileDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public virtual ICollection<CategoryDTO> Categories { get; set; }
    }
}

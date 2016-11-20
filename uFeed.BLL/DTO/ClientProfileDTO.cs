using System.Collections.Generic;
using uFeed.BLL.Enums;

namespace uFeed.BLL.DTO
{
    public class ClientProfileDTO
    {
        public ClientProfileDTO()
        {
            Logins = new List<Socials>();
            Categories = new List<CategoryDTO>();
        }

        public int UserId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public ICollection<Socials> Logins { get; set; }

        public byte[] Photo { get; set; }

        public ICollection<CategoryDTO> Categories { get; set; }
    }
}

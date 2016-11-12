using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using uFeed.DAL.Enums;

namespace uFeed.DAL.Entities
{
    public class ClientProfile : BaseType
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        public ICollection<Socials> Logins { get; set; }

        public byte[] Photo { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
    }
}

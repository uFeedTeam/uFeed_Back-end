using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace uFeed.Entities
{
    public class ClientProfile : BaseType
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }        

        public byte[] Photo { get; set; }

        public virtual ICollection<Login> Logins { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
    }
}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace uFeed.Entities
{
    public class User : BaseType
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public byte[] Photo { get; set; }

        public virtual ICollection<ClientBookmark> Bookmarks { get; set; }

        public virtual ICollection<Login> Logins { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
    }
}
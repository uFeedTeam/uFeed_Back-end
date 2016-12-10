using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace uFeed.Entities
{
    public class ClientProfile : BaseType
    {
        [Required]
        [Index("IX_userId", 1, IsUnique = true)]
        public int UserId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        public byte[] Photo { get; set; }

        public virtual ICollection<ClientBookmark> Bookmarks { get; set; }

        public virtual ICollection<Login> Logins { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
    }
}
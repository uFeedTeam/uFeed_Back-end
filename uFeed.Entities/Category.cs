using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace uFeed.Entities
{
    public class Category : BaseType
    {
        [Required]
        public string Name { get; set; }

        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual ClientProfile User { get; set; }

        public ICollection<SocialAuthor> Authors { get; set; }
    }
}
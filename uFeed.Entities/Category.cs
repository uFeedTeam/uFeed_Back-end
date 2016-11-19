using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace uFeed.Entities
{
    public class Category : BaseType
    {
        [Required]
        public string Name { get; set; }

        public int ClientProfileId { get; set; }

        [ForeignKey(nameof(ClientProfileId))]
        public virtual ClientProfile User { get; set; }

        public virtual ICollection<SocialAuthor> Authors { get; set; }
    }
}
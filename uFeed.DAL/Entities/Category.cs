using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using uFeed.DAL.Entities.Social;

namespace uFeed.DAL.Entities
{
    public class Category
    {
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual ClientProfile User { get; set; }

        public virtual ICollection<Author> Authors { get; set; }
    }
}
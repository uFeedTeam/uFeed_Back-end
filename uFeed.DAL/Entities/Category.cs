﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public ICollection<CategoryAuthor> Authors { get; set; }
    }
}
﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace uFeed.DAL.Entities
{
    public class ClientProfile : BaseType
    {
        [Required]
        public string Name { get; set; }

        public byte[] Photo { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
    }
}

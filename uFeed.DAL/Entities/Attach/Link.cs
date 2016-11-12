﻿using uFeed.DAL.Entities.Social;

namespace uFeed.DAL.Entities.Attach
{
    public class Link
    {
        public string Description { get; set; }

        public Photo Photo { get; set; }

        public string Url { get; set; }
    }
}
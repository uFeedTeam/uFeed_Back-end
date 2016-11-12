using System.Collections.Generic;

namespace uFeed.BLL.DTO.Social.Attach
{
    public class AlbumDTO: List<PhotoDTO>
    {
        public string Description { get; set; }
    }
}
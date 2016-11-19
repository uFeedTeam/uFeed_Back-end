using uFeed.DAL.SocialService.Models.FacebookModel.Common.Picture;

namespace uFeed.DAL.SocialService.Models.FacebookModel.User
{
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string First_name { get; set; }
        public string Last_name { get; set; }
        public Picture Picture { get; set; }    
    }
}
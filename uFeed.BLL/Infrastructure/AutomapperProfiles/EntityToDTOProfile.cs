using System.Linq;
using AutoMapper;
using uFeed.BLL.DTO;
using uFeed.BLL.DTO.Social;
using uFeed.BLL.DTO.Social.Attach;
using uFeed.BLL.Enums;
using uFeed.Entities;
using uFeed.Entities.Social;
using uFeed.Entities.Social.Attach;

namespace uFeed.BLL.Infrastructure.AutomapperProfiles
{
    public class EntityToDTOProfile : Profile
    {
        public EntityToDTOProfile()
        {
            CreateMap<ClientProfile, ClientProfileDTO>().BeforeMap((source, dto) =>
            {
                dto.Logins = source.Logins.Select(l => (Socials)l.LoginType).ToList();
            }).ForMember(dto => dto.Logins, expression => expression.Ignore());
            CreateMap<Category, CategoryDTO>().BeforeMap((source, dto) =>
            {
                dto.UserId = source.User.UserId;
            });
            CreateMap<SocialAuthor, SocialAuthorDTO>();

            CreateMap<Author, AuthorDTO>();
            CreateMap<Photo, PhotoDTO>();
            CreateMap<Post, PostDTO>();
            CreateMap<Attachment, AttachmentDTO>();
            CreateMap<Album, AlbumDTO>();
            CreateMap<Document, DocumentDTO>();
            CreateMap<Likes, LikesDTO>();
            CreateMap<Link, LinkDTO>();
            CreateMap<Poll, PollDTO>();
            CreateMap<PollAnswer, PollAnswerDTO>();
            CreateMap<Video, VideoDTO>();
        }
    }
}
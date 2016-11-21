using AutoMapper;
using uFeed.BLL.DTO;
using uFeed.BLL.DTO.Social;
using uFeed.BLL.DTO.Social.Attach;
using uFeed.Entities;
using uFeed.Entities.Social;
using uFeed.Entities.Social.Attach;

namespace uFeed.BLL.Infrastructure.AutomapperProfiles
{
    public class DTOToEntityProfile : Profile
    {
        public DTOToEntityProfile()
        {
            CreateMap<ClientProfileDTO, ClientProfile>()
                .ForMember(profile => profile.Logins, expression => expression.Ignore());
            CreateMap<CategoryDTO, Category>();
            CreateMap<SocialAuthorDTO, SocialAuthor>();

            CreateMap<AuthorDTO, Author>();
            CreateMap<PhotoDTO, Photo>();
            CreateMap<PostDTO, Post>();
            CreateMap<AttachmentDTO, Attachment>();
            CreateMap<AlbumDTO, Album>();
            CreateMap<DocumentDTO, Document>();
            CreateMap<LikesDTO, Likes>();
            CreateMap<LinkDTO, Link>();
            CreateMap<PollDTO, Poll>();
            CreateMap<PollAnswerDTO, PollAnswer>();
            CreateMap<VideoDTO, Video>();
        }
    }
}
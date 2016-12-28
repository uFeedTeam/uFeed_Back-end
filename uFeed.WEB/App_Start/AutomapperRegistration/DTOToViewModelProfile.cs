using AutoMapper;
using uFeed.BLL.DTO;
using uFeed.BLL.DTO.Social;
using uFeed.BLL.DTO.Social.Attach;
using uFeed.WEB.ViewModels;
using uFeed.WEB.ViewModels.Social;
using uFeed.WEB.ViewModels.Social.Attach;

namespace uFeed.WEB.AutomapperRegistration
{
    public class DTOToViewModelProfile : Profile
    {
        public DTOToViewModelProfile()
        {            
            CreateMap<UserDTO, UserViewModel>();
            CreateMap<CategoryDTO, CategoryViewModel>();
            CreateMap<SocialAuthorDTO, SocialAuthorViewModel>();

            CreateMap<AuthorDTO, AuthorViewModel>();
            CreateMap<PhotoDTO, PhotoViewModel>();
            CreateMap<PostDTO, PostViewModel>();
            CreateMap<AttachmentDTO, AttachmentViewModel>();
            CreateMap<AlbumDTO, AlbumViewModel>();
            CreateMap<DocumentDTO, DocumentViewModel>();
            CreateMap<LikesDTO, LikesViewModel>();
            CreateMap<LinkDTO, LinkViewModel>();
            CreateMap<PollDTO, PollViewModel>();
            CreateMap<PollAnswerDTO, PollAnswerViewModel>();
            CreateMap<VideoDTO, VideoViewModel>();
        }
    }
}
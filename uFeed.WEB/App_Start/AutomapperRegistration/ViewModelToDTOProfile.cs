using AutoMapper;
using uFeed.BLL.DTO;
using uFeed.BLL.DTO.Social;
using uFeed.BLL.DTO.Social.Attach;
using uFeed.WEB.ViewModels;
using uFeed.WEB.ViewModels.Social;
using uFeed.WEB.ViewModels.Social.Attach;

namespace uFeed.WEB.AutomapperRegistration
{
    public class ViewModelToDTOProfile : Profile
    {
        public ViewModelToDTOProfile()
        {
            CreateMap<UserViewModel, UserDTO>();
            CreateMap<CategoryViewModel, CategoryDTO>();
            CreateMap<SocialAuthorViewModel, SocialAuthorDTO>();

            CreateMap<AuthorViewModel, AuthorDTO>();
            CreateMap<PhotoViewModel, PhotoDTO>();
            CreateMap<PostViewModel, PostDTO>();
            CreateMap<AttachmentViewModel, AttachmentDTO>();
            CreateMap<AlbumViewModel, AlbumDTO>();
            CreateMap<DocumentViewModel, DocumentDTO>();
            CreateMap<LikesViewModel, LikesDTO>();
            CreateMap<LinkViewModel, LinkDTO>();
            CreateMap<PollViewModel, PollDTO>();
            CreateMap<PollAnswerViewModel, PollAnswerDTO>();
            CreateMap<VideoViewModel, VideoDTO>();
        }
    }
}
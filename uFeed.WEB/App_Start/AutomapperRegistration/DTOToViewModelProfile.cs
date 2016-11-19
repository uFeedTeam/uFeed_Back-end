using AutoMapper;
using uFeed.BLL.DTO;
using uFeed.WEB.ViewModels;

namespace uFeed.WEB.AutomapperRegistration
{
    public class DTOToViewModelProfile : Profile
    {
        public DTOToViewModelProfile()
        {            
            CreateMap<ClientProfileDTO, ClientProfileViewModel>();
            CreateMap<CategoryDTO, CategoryViewModel>();
        }
    }
}
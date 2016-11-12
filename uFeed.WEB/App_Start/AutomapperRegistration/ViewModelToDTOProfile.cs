using AutoMapper;
using uFeed.BLL.DTO;
using uFeed.WEB.ViewModels;

namespace uFeed.WEB.AutomapperRegistration
{
    public class ViewModelToDTOProfile : Profile
    {
        public ViewModelToDTOProfile()
        {
            CreateMap<ClientProfileViewModel, ClientProfileDTO>();
            CreateMap<CategoryViewModel, CategoryDTO>();
        }
    }
}
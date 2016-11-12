using AutoMapper;
using uFeed.BLL.DTO;
using uFeed.DAL.Entities;

namespace uFeed.BLL.Infrastructure.AutomapperProfiles
{
    public class EntityToDTOProfile : Profile
    {
        public EntityToDTOProfile()
        {
            CreateMap<ClientProfile, ClientProfileDTO>();
            CreateMap<Category, CategoryDTO>();
        }
    }
}
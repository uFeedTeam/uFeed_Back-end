using AutoMapper;
using uFeed.BLL.DTO;
using uFeed.DAL.Entities;

namespace uFeed.BLL.Infrastructure.AutomapperProfiles
{
    public class DTOToEntityProfile : Profile
    {
        public DTOToEntityProfile()
        {
            CreateMap<ClientProfileDTO, ClientProfile>();
            CreateMap<CategoryDTO, Category>();
        }
    }
}
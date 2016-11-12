using System.Linq;
using AutoMapper;
using uFeed.BLL.DTO;
using uFeed.BLL.Enums;
using uFeed.DAL.Entities;

namespace uFeed.BLL.Infrastructure.AutomapperProfiles
{
    public class EntityToDTOProfile : Profile
    {
        public EntityToDTOProfile()
        {
            CreateMap<ClientProfile, ClientProfileDTO>().BeforeMap((source, dto) =>
            {
                dto.Logins = source.Logins.Select(l => (Socials)l.LoginType).ToList();
            });
            CreateMap<Category, CategoryDTO>();
        }
    }
}
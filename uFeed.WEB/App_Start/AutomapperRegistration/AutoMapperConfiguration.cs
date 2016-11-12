using AutoMapper;
using uFeed.BLL.Infrastructure.AutomapperProfiles;

namespace uFeed.WEB.AutomapperRegistration
{
    public static class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new ViewModelToDTOProfile());
                cfg.AddProfile(new DTOToViewModelProfile());
                cfg.AddProfile(new EntityToDTOProfile());
                cfg.AddProfile(new DTOToEntityProfile());
            });
        }
    }   
}
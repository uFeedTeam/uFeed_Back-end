using Ninject.Modules;
using uFeed.DAL.Interfaces;
using uFeed.DAL.SocialService.Interfaces;
using uFeed.DAL.SocialService.UnitOfWorks;
using uFeed.DAL.UnitOfWorks;

namespace uFeed.BLL.Infrastructure
{
    public class ServiceModule : NinjectModule
    {
        private readonly string _connectionString;

        public ServiceModule(string connection)
        {
            _connectionString = connection;
        }

        public override void Load()
        {
            Bind<IUnitOfWork>().To<UFeedUnitOfWork>().WithConstructorArgument(_connectionString);
            Bind<ISocialUnitOfWork>().To<SocialUnitOfWork>();
        }
    }
}
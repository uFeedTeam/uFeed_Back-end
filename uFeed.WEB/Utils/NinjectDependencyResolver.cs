using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Web.Mvc;
using Ninject;
using Ninject.Web.Common;
using uFeed.BLL.Interfaces;
using uFeed.BLL.Services;
using uFeed.WEB.Account.Implementation;
using uFeed.WEB.Account.Interfaces;

namespace uFeed.WEB.Utils
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private readonly IKernel _kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            _kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            _kernel.Bind<IUserService>().To<UserService>();
            _kernel.Bind<ISocialService>().To<SocialService>();
            _kernel.Bind<ICategoryService>().To<CategoryService>();
            _kernel.Bind<IAuthentication>().To<Authentication>().InRequestScope();
            _kernel.Bind<IIdentity>().To<UserIdentity>();
            _kernel.Bind<IPrincipal>().To<UserProvider>();
        }
    }
}
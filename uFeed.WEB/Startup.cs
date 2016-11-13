using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(uFeed.WEB.Startup))]

namespace uFeed.WEB
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

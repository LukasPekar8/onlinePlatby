using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(onlinePlatby.Startup))]
namespace onlinePlatby
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

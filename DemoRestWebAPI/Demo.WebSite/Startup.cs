using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Demo.WebSite.Startup))]
namespace Demo.WebSite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

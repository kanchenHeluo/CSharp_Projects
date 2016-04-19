using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MemoWebApp.Startup))]
namespace MemoWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

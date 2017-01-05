using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(jsmvc.Startup))]
namespace jsmvc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

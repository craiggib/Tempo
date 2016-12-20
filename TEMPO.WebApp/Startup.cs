using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TEMPO.WebApp.Startup))]
namespace TEMPO.WebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

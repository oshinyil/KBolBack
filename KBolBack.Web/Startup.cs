using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(KBolBack.Web.Startup))]
namespace KBolBack.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

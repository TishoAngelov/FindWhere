using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FindWhere.Web.Startup))]
namespace FindWhere.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(University.API.Startup))]
namespace University.API
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

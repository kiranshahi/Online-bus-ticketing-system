using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BusTicketing.Startup))]
namespace BusTicketing
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

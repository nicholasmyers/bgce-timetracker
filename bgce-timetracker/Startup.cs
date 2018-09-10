using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(bgce_timetracker.Startup))]
namespace bgce_timetracker
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

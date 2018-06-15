using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BugTrackerBD.Startup))]
namespace BugTrackerBD
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

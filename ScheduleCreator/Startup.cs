using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ScheduleCreator.Startup))]
namespace ScheduleCreator
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}

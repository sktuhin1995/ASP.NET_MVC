using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Laptop_Technology.Startup))]
namespace Laptop_Technology
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

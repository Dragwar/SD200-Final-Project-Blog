using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SD200_Final_Project_Blog.Startup))]
namespace SD200_Final_Project_Blog
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

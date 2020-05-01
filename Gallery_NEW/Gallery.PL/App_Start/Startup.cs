using System;
using System.Data.Entity;
using System.Web.Http;
using Gallery.DAL.Model;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

[assembly: OwinStartup(typeof(Gallery.PL.App_Start.Startup))]

namespace Gallery.PL.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "ApplicationCookie",
                LoginPath = new PathString("/Account/Login"),
                SlidingExpiration = true,
                ExpireTimeSpan = TimeSpan.FromMinutes(60)
            });

            DIConfig.Configure(new HttpConfiguration());

            Database.SetInitializer(new UserDbInitializer());
        }
    }
}

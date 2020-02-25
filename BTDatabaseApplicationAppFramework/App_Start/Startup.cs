using System;
using System.Web.Mvc;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BTDatabaseApplicationAppFramework.Startup))]

namespace BTDatabaseApplicationAppFramework
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }

        internal static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            throw new NotImplementedException();
        }
    }
}
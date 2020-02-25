using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using log4net.Config;
using log4net;



namespace BTDatabaseApplicationAppFramework
{
    public class MvcApplication : System.Web.HttpApplication
    {
        //private static readonly ILog log = LogManager.GetLogger(typeof(MvcApplication));
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(MvcApplication));
    //private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected void Application_Start()
        {

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            //Startup.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
           log4net.Config.XmlConfigurator.Configure();
           logger.Info("Starting the application at ..."+ DateTime.Now);
            
        }
    }
}

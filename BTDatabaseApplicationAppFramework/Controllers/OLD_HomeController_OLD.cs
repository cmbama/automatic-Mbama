using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using BTDatabaseApplicationAppFramework;
using BTDatabaseApplicationAppFramework.Models;

namespace BTDatabaseApplicationAppFramework.Controllers
{
    public class OLD_HomeController_OLD : Controller
    {
        public ActionResult Index()
        {
            return View();
        }



        public ActionResult DDIDisplay()
        {
            //return RedirectToAction("DDIDisplay");
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
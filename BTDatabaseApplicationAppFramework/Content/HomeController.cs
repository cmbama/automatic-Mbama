using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BTDatabaseApplicationAppFramework.Controllers
{
    public class HomeControllerOLD : Controller
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
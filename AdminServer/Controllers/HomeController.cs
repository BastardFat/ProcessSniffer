using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminServer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult Config()
        {
            ViewBag.Title = "Configuration Screen";

            return View(Models.ClientConfig.Saved??(new Models.ClientConfig()));
        }

        public ActionResult ConfigsSaved(Models.ClientConfig cc)
        {

            Models.ClientConfig.Saved = cc;

            ViewBag.Title = "Sucessful";

            return View();
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConseilApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Je vous présente le site de conseil en mode...";
            ViewBag.Page = "Home";
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "A propos de mon site...";
            ViewBag.Page = "About";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact";
            ViewBag.Page = "Contact";

            return View();
        }
    }
}

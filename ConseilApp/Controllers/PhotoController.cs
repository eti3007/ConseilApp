using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConseilApp.Controllers
{
    public class PhotoController : Controller
    {
        private const string VetementFolder = "~/Images/Photo/Vetement/";
        private const string HabillageFolder = "~/Images/Photo/Habillage/";
        //
        // GET: /Photo/

        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult AfficheHorizontal(List<string> urls)
        {
            return PartialView(urls);
        }
    }
}

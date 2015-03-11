using ConseilApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConseilApp.Controllers
{
    public class HomeController : BaseController
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

        public ActionResult Menu(string pageEnCours)
        {
            // récupère le type de l'utilisateur dans la variable de session :
            var statut = base.GetSession<int>(SessionKey.PersonneStatut);

            MenuViewModel model = new MenuViewModel();
            model.Page = pageEnCours;
            var menus = new List<Menu>();

            menus.Add(new Menu() { Controller = "Home", Action = "Index", PageName = "Home", Texte = "Home" });
            if (statut > 0)
                menus.Add(new Menu() { Controller = "Recherche", Action = "Demandes", PageName = "Demandes", Texte = "Mes demandes" });
            if (statut > 2)
                menus.Add(new Menu() { Controller = "Recherche", Action = "Propositions", PageName = "Propositions", Texte = "Mes propositions" });
            menus.Add(new Menu() { Controller = "Home", Action = "About", PageName = "About", Texte = "A propos" });
            menus.Add(new Menu() { Controller = "Home", Action = "Contact", PageName = "Contact", Texte = "Contactez nous" });
                        
            model.Menus = menus;
            
            return PartialView("_Menu", model);
        }
    }
}

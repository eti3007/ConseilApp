using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ConseilApp.Builders.Interfaces;
using ConseilBLL.Interfaces;

namespace ConseilApp.Controllers
{
    public class HabillageController : Controller
    {
        private IHabillageBuilder _HabillageBuilder;

        public HabillageController(IHabillageBuilder HabillageBuilder)
        {
            this._HabillageBuilder = HabillageBuilder;
        }

        [Authorize]
        public ActionResult Visualiser(int style, int personne)
        {
            ViewBag.Page = "Visualiser";
            ViewBag.Title = "Visualiser les habillages";
            ViewBag.Style = style;
            ViewBag.Personne = personne;
            ViewBag.IsConception = false;
            var model = this._HabillageBuilder.RecupereConseil(style, personne, true);
            return View(model);
        }

        [Authorize]
        public ActionResult Concevoir(int style, int personne)
        {
            ViewBag.Page = "Concevoir";
            ViewBag.Title = "Concevoir un habillage";
            ViewBag.Style = style;
            ViewBag.Personne = personne;
            ViewBag.IsConception = true;
            var model = this._HabillageBuilder.RecupereConseil(style, personne, false);
            return View(model);
        }

        [Authorize]
        public PartialViewResult ListeHabillage(int conseilId)
        {
            // A - récupère la liste des habillages du conseil sélectionné

            // B - récupère la liste complète des photos par habillage


            return PartialView("_VoirHabillage");
        }

        [Authorize]
        public PartialViewResult ConcevoirHabillage(int conseilId, int style, int abonne)
        {
            Models.Habillage.ConceptionModel model = new Models.Habillage.ConceptionModel();
            // A - récupère la liste des types de vêtement
            model.typeVetementListe = this._HabillageBuilder.RecupereListeTypesVetement();

            // B - récupère la liste des vêtements de l'abonné pour le style du conseil sélectionné
            model.vetements = this._HabillageBuilder.RecupereAbonnePhotoVetementDispoParStyle(style, abonne);

            return PartialView("_CreerHabillage", model);
        }

        [Authorize]
        public PartialViewResult VisualiserHabillage(int conseilId)
        {
            return null;
        }

        [Authorize]
        public void ClotureConseil(int conseilId) { }

        [Authorize]
        public PartialViewResult AfficheConseils(int style, int personne, bool isVisualisation)
        {
            ViewBag.IsConception = !isVisualisation;
            var model = this._HabillageBuilder.RecupereConseil(style, personne, isVisualisation);
            return PartialView("_AfficheTableau", model);
        }

        [Authorize]
        public PartialViewResult Message()
        {
            return null;
        }
    }
}

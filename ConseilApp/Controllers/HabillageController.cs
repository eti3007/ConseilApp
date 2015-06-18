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
        public ActionResult Concevoir(int style, int personne)
        {
            var model = this._HabillageBuilder.RecupereConseil(style, personne, false);
            return View(model);
        }

        [Authorize]
        public ActionResult Visualiser(int style, int personne)
        {
            var model = this._HabillageBuilder.RecupereConseil(style, personne, true);
            return View(model);
        }


        [Authorize]
        public PartialViewResult ListeHabillage()
        {
            return null;
        }

        [Authorize]
        public PartialViewResult ConcevoirHabillage()
        {
            // A - Vérifie que les photos pour les types de vêtement obligatoire soient dans la liste
            // A - Récupère l'ID des photos choisies
            return null;
        }

        [Authorize]
        public PartialViewResult VisualiserHabillage()
        {
            return null;
        }

        [Authorize]
        public PartialViewResult Message()
        {
            return null;
        }
    }
}

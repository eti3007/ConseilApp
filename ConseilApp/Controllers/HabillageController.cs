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
        private IHabillageService _HabillageService;

        public HabillageController(IHabillageService HabillageService)
        {
            this._HabillageService = HabillageService;
        }

        [Authorize]
        public PartialViewResult ListeConseils()
        {
            return null;
        }

        [Authorize]
        public PartialViewResult ListeHabillage()
        {
            return null;
        }

        [Authorize]
        public PartialViewResult ConcevoirHabillage()
        {
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

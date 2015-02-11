using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ConseilApp.Builders.Interfaces;
using ConseilApp.Models.Recherche;

namespace ConseilApp.Controllers
{
    public class RechercheController : BaseController
    {
        private IRechercheBuilder _RechercheBuilder;

        public RechercheController(IRechercheBuilder RechercheBuilder)
        {
            this._RechercheBuilder = RechercheBuilder;
        }

        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Demandes()
        {
            MenuDemandeEnregistreCookie();
            ViewBag.Page = "Demande";
            ViewBag.Title = "Gérer vos demandes";

            //*************************************************
            // créer l'objet du model
            RechercheModel demandes = GenereNewModel();

            // initialise style et personne Id
            this._RechercheBuilder.SetPersonneStyleIdentifier(base.PersonneId, base.StyleEnCours);

            // récupère la liste des attentes
            demandes.premiereListe.apteConseil = _RechercheBuilder.DemandeAbonnePeutAider();

            // récupère la liste des soutiens
            demandes.deuxiemeListe.proposeAide = _RechercheBuilder.DemandeAbonneProposeAider();

            //*************************************************

            return View(demandes);
        }

        [Authorize]
        public ActionResult Propositions()
        {
            // on vérifie que l'utilisateur connecté soit un conseillé !
            var statut = base.GetSession<int>(SessionKey.PersonneStatut);
            if ((ConseilOBJ.PersonneStatus)statut != ConseilOBJ.PersonneStatus.Conseiller)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                MenuPropositionEnregistreCookie();
                ViewBag.Page = "Proposition";
                ViewBag.Title = "Gérer vos propositions";

                //*************************************************
                // créer l'objet du model
                RechercheModel propositions = GenereNewModel();

                // initialise style et personne Id
                this._RechercheBuilder.SetPersonneStyleIdentifier(base.PersonneId, base.StyleEnCours);

                // récupère la liste des attentes
                propositions.premiereListe.attenteConseil = _RechercheBuilder.PropositionAbonneAttenteConseil();

                // récupère la liste des soutiens
                propositions.deuxiemeListe.solliciteAide = _RechercheBuilder.PropositionAbonneSolliciteAide();
                //*************************************************

                return View(propositions);
            }
        }

        private RechercheModel GenereNewModel()
        {
            var obj = new RechercheModel();
            obj.premiereListe = new PremiereListe();
            obj.deuxiemeListe = new DeuxiemeListe();
            return obj;
        }

        [Authorize]
        public PartialViewResult Notification()
        {
            bool? isRechercheDemande = IsRechercheDemandeMenu();

            if (!isRechercheDemande.HasValue) return null;      // redirecte to Home

            if (isRechercheDemande.Value) { }
            else { }

            return new PartialViewResult();     // View();
        }
    }
}

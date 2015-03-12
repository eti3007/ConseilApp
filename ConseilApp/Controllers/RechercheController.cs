using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ConseilApp.Builders.Interfaces;
using ConseilApp.Models.Recherche;
using ConseilBLL.Interfaces;

namespace ConseilApp.Controllers
{
    public class RechercheController : BaseController
    {

        private IRechercheBuilder _RechercheBuilder;
        private IStyleService _StyleService;

        public RechercheController(IStyleService StyleService, IRechercheBuilder RechercheBuilder)
        {
            this._StyleService = StyleService;
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
            ViewBag.Page = "Demandes";
            ViewBag.Title = "Gérer vos demandes";
            ViewBag.ListeStyle = base.ListeDesStyles.OrderBy(x => x.Nom).ToList();
            ViewBag.RechercheStyleEncours = base.ListeDesStyles[0].Id;
            
            return View(GetRechercheModel(base.ListeDesStyles[0].Id));
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
                List<ConseilOBJ.Style> lstStyleConseiller = this._StyleService.RecupereListeDesStylesConseiller(base.GetSession<int>(SessionKey.PersonneID));
                if (lstStyleConseiller==null || lstStyleConseiller.Count <= 0) return RedirectToAction("Index", "Home");

                MenuPropositionEnregistreCookie();                
                ViewBag.Page = "Propositions";
                ViewBag.Title = "Gérer vos propositions";
                ViewBag.ListeStyle = lstStyleConseiller;
                ViewBag.RechercheStyleEncours = lstStyleConseiller[0].Id;

                return View(GetRechercheModel(lstStyleConseiller[0].Id, false));
            }
        }
        
        [Authorize]
        public PartialViewResult RecherchePartial(int style, string partialName, string pageName)
        {
            ViewBag.RechercheStyleEncours = style;
            return PartialView(partialName, GetRechercheModel(style, (pageName.Equals("Demandes")), partialName));
        }

        #region METHODES PRIVEES
        private RechercheModel GetRechercheModel(int style, bool demande = true, string partialToUpd = "")
        {
            // créer l'objet du model
            RechercheModel recherche = GenereNewModel();

            // initialise style et personne Id
            this._RechercheBuilder.SetPersonneStyleIdentifier(base.PersonneId, style);

            if (demande) {
                if (partialToUpd.Equals("_ListeEnAttente")) {
                    // récupère la liste des attentes
                    recherche.premiereListe.apteConseil = _RechercheBuilder.DemandeAbonnePeutAider(); 
                }
                else if (partialToUpd.Equals("_ListeEnSoutien")) {
                    // récupère la liste des soutiens
                    recherche.deuxiemeListe.proposeAide = _RechercheBuilder.DemandeAbonneProposeAider(); 
                }
                else {
                    // récupère la liste des attentes
                    recherche.premiereListe.apteConseil = _RechercheBuilder.DemandeAbonnePeutAider();

                    // récupère la liste des soutiens
                    recherche.deuxiemeListe.proposeAide = _RechercheBuilder.DemandeAbonneProposeAider();
                }
            }
            else {
                if (partialToUpd.Equals("_ListeEnAttente")) {
                    // récupère la liste des attentes
                    recherche.premiereListe.attenteConseil = _RechercheBuilder.PropositionAbonneAttenteConseil();
                }
                else if (partialToUpd.Equals("_ListeEnSoutien")) {
                    // récupère la liste des soutiens
                    recherche.deuxiemeListe.solliciteAide = _RechercheBuilder.PropositionAbonneSolliciteAide();
                }
                else {
                    // récupère la liste des attentes
                    recherche.premiereListe.attenteConseil = _RechercheBuilder.PropositionAbonneAttenteConseil();

                    // récupère la liste des soutiens
                    recherche.deuxiemeListe.solliciteAide = _RechercheBuilder.PropositionAbonneSolliciteAide();
                }
            }

            return recherche;
        }

        private RechercheModel GenereNewModel()
        {
            var obj = new RechercheModel();
            obj.premiereListe = new PremiereListe();
            obj.deuxiemeListe = new DeuxiemeListe();
            return obj;
        }
        #endregion

        [Authorize]
        public PartialViewResult Notification()
        {
            bool? isRechercheDemande = IsRechercheDemandeMenu();

            if (!isRechercheDemande.HasValue) return null;      // redirecte to Home

            if (isRechercheDemande.Value) { }
            else { }

            return new PartialViewResult();     // View();
        }

        [HttpPost]
        public string ValiderAide(int? conseilId, int? demandeurId, int? conseillerId, int styleId)
        {
            return "";
        }
    }
}

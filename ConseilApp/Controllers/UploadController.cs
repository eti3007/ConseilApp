﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ConseilApp.Models;
using ConseilOBJ;
using ConseilApp.Builders.Interfaces;
using ConseilBLL.Interfaces;

namespace ConseilApp.Controllers
{
    public class UploadController : BaseController
    {
        private IStyleService _StyleService;
        private IVetementService _VetementService;
        private IStatutHistoriqueService _StatutHistoriqueService;
        private IPhotoService _PhotoService;
        private IPhotoBuilder _PhotoBuilder;

        public UploadController(IPhotoBuilder PhotoBuilder, IPhotoService PhotoService,
                                IStyleService StyleService, IVetementService VetementService, IStatutHistoriqueService StatutHistoriqueService)
        {
            this._StyleService = StyleService;
            this._VetementService = VetementService;
            this._StatutHistoriqueService = StatutHistoriqueService;
            this._PhotoService = PhotoService;
            this._PhotoBuilder = PhotoBuilder;
        }

        [Authorize]
        public ActionResult UploadPhotos(int vetementStyleId, int vetementId, int habillageStyleId)
        {
            // Récupère le nombre maximum de photo téléchargable par type de vêtement :
            var MaxPhoto = System.Configuration.ConfigurationManager.AppSettings["MaxPhotoParTypeVetement"];

            UploadPhotoViewModel model = PrepareViewModel(vetementStyleId, vetementId, habillageStyleId);

            // vérifier le nombre maximum de photo pour masquer la DIV qui contient l'upload (utilise ViewBag)
            this.ViewBag.MaxPhoto = MaxPhoto;
            this.ViewBag.NbPhotos = 0;  // Il faut gérer la limite du nombre de photo de vêtement par style

            return View(model);
        }

        //[Authorize]
        //public ActionResult UploadVetement(int styleId, int vetementId)
        //{
        //    // Récupère le nombre maximum de photo téléchargable par type de vêtement :
        //    var MaxPhoto = System.Configuration.ConfigurationManager.AppSettings["MaxPhotoParTypeVetement"];
            
        //    if (vetementId == 0 && styleId == 0) styleId = base.StyleEnCours;

        //    UploadVetementViewModel model = null;//PrepareViewModel(styleId);

        //    // vérifier le nombre maximum de photo pour masquer la DIV qui contient l'upload (utilise ViewBag)
        //    this.ViewBag.MaxPhoto = MaxPhoto;
        //    this.ViewBag.UploadOK = "true";
        //    this.ViewBag.NbPhotos = 0;  // Il faut gérer la limite du nombre de photo de vêtement par style

        //    return View(model);
        //}

        [HttpPost]
        [Authorize]
        public ActionResult UploadPhotos(UploadPhotoViewModel model)
        {
            if (Request.Files.Count > 0)
            {
                if (model.VetementValidation) UploadVetement(model);
                else UploadHabillage(model);
            }

            // Recharge les photos sur la même page
            return this.UploadPhotos(model.VetementValidation ? Int32.Parse(model.PhotoVetement.Style) : 0,
                model.VetementValidation ? Int32.Parse(model.PhotoVetement.Vetement) : 0, 
                model.EstConseiller ? Int32.Parse(model.PhotoHabillage.Style) : 0);
        }

        private void UploadVetement(UploadPhotoViewModel model)
        {
            // sauvegarde les images des vêtements
            var fileUpload = new FileUpload(Request.Files, base.PersonneId, Int32.Parse(model.PhotoVetement.Style), true);
            List<string> listPhoto = fileUpload.UploadWholeFile();

            if (listPhoto != null && listPhoto.Count > 0)
            {
                // sauvegarde les urls pour la personne connectée
                this._PhotoService.AjoutePhotoVetement(Int32.Parse(model.PhotoVetement.Style), model.PhotoVetement.ModeAttente, base.PersonneId, Int32.Parse(model.PhotoVetement.Vetement), listPhoto);

                // TODO : logue les infos des photos sauvegardées par personne et par style

            }
        }

        private void UploadHabillage(UploadPhotoViewModel model)
        {
            // sauvegarde les images des habillages
            var fileUpload = new FileUpload(Request.Files, base.PersonneId, Int32.Parse(model.PhotoHabillage.Style), false);
            List<string> listPhoto = fileUpload.UploadWholeFile();

            if (listPhoto != null && listPhoto.Count > 0)
            {
                // sauvegarde les urls pour la personne connectée
                this._PhotoService.AjoutePhotoHabillage(Int32.Parse(model.PhotoHabillage.Style), base.PersonneId, listPhoto);

                // TODO : logue les infos des photos sauvegardées par personne et par style

            }
        }

        [Authorize]
        public PartialViewResult AffichePhotosVetement(string style, string vetement)
        {
            int vetementId = string.IsNullOrEmpty(vetement) ? 0 : Convert.ToInt32(vetement);
            int styleId = string.IsNullOrEmpty(style) ? 0 : Convert.ToInt32(style);
            List<string> UrlListe = new List<string>();
            if (vetementId > 0)
                UrlListe = this._PhotoBuilder.UrlPhotoListe(this._PhotoService.RecuperePhotosPourPersonneStyleVetement(base.PersonneId, styleId, vetementId));
            else
                UrlListe = this._PhotoBuilder.UrlPhotoListe(this._PhotoService.RecuperePhotosPourPersonneStyle(base.PersonneId, PhotoType.Vetement, styleId));

            if (UrlListe.Count > 0)
            {
                var fileUpload = new FileUpload(base.PersonneId, styleId, true);
                UrlListe = fileUpload.CompleteUrlPicture(UrlListe, true);
                fileUpload = null;
            }

            // Il faut gérer la limite du nombre de photo de vêtement par style

            return PartialView("_AffichePhoto", UrlListe);
        }

        [Authorize]
        public PartialViewResult AffichePhotosHabillage(string style, string personne)
        {
            int styleId = string.IsNullOrEmpty(style) ? 0 : Convert.ToInt32(style);
            if (styleId == 0) styleId = base.StyleEnCours;
            List<string> UrlListe;

            int personneId = !string.IsNullOrEmpty(personne) ? Convert.ToInt32(personne) : base.PersonneId;

            UrlListe = this._PhotoBuilder.UrlPhotoListe(this._PhotoService.RecuperePhotosPourPersonneStyle(personneId, PhotoType.Habille, styleId));

            if (UrlListe.Count > 0)
            {
                var fileUpload = new FileUpload(personneId, styleId, false);
                UrlListe = fileUpload.CompleteUrlPicture(UrlListe, false);
                fileUpload = null;
            }

            // Il faut gérer la limite du nombre de photo de vêtement par style
            return PartialView("_AffichePhoto", UrlListe);
        }

        [Authorize]
        public void SupprimeImage(string nom, string style, string vetement)
        {
            int vetementId = string.IsNullOrEmpty(vetement) ? 0 : Convert.ToInt32(vetement);
            int styleId = string.IsNullOrEmpty(style) ? 0 : Convert.ToInt32(style);

            if (vetementId == 0 && styleId == 0) return;

            var fileUpload = new FileUpload(base.PersonneId, styleId, string.IsNullOrEmpty(vetement) ? false : true);
            string s = fileUpload.GetPictureNameFromUrl(nom);

            bool imageSupp = string.IsNullOrEmpty(vetement) ?  
                this._PhotoService.SupprimePhotosHabillageParPersonneStyle(base.PersonneId, styleId, s) :
                this._PhotoService.SupprimePhotosParPersonneStyleVetement(base.PersonneId, styleId, vetementId, s);
            
            if (imageSupp)
            {
                // supprime physiquement l'image
                if (!fileUpload.DeleteFile(nom))
                {
                    // logue l'echec de la suppression du fichier physiquement
                }
            }
        }

        /// <summary>
        /// Initialise les models pour l'upload des vêtements et des habillages
        /// </summary>
        /// <param name="styleId"></param>
        /// <returns></returns>
        private UploadPhotoViewModel PrepareViewModel(int vetementStyleId, int vetementId, int habillageStyleId)
        {
            UploadPhotoViewModel result = new UploadPhotoViewModel();
            result.EstConseiller = false;

            // model pour les photos de vêtement
            UploadVetementViewModel modelVetement = new UploadVetementViewModel();
            var listeStyles = base.ListeDesStyles;
            if (listeStyles == null) base.SetSession(SessionKey.ListeStyle, this._StyleService.RecupereListeDesStyles());
            modelVetement.styleListe = DropDownListBuilder<DropDownListeStyle>.CreateDropDownList(this._StyleService.RecupereListeDesStylesPourDDL());
            modelVetement.vetementListe = DropDownListBuilder<DropDownListeVetement>.CreateDropDownList(this._VetementService.RecupereListeDesVetementsPourDDL());
            modelVetement.ModeAttente = this._StatutHistoriqueService.RecupereStatusPourPersonneEtStyle(base.PersonneId, vetementStyleId) == (int)PersonneStatus.EnAttente;
            modelVetement.Style = vetementStyleId != 0 ? vetementStyleId.ToString() : base.StyleEnCours.ToString();
            modelVetement.Vetement = vetementId != 0 ? vetementStyleId.ToString() : "";
            result.PhotoVetement = modelVetement;

            // model pour les photos d'habillage
            var statutPersonne = base.GetSession<int>(SessionKey.PersonneStatut);
            if ((int)PersonneStatus.Conseiller == statutPersonne)
            {
                result.EstConseiller = true;
                UploadHabillageViewModel modelHabillage = new UploadHabillageViewModel();
                modelHabillage.styleListe = modelVetement.styleListe;
                modelHabillage.Style = habillageStyleId != 0 ? habillageStyleId.ToString() : base.StyleEnCours.ToString();
                result.PhotoHabillage = modelHabillage;
            }

            return result;
        }
    }
}

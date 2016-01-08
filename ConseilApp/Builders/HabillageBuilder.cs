using System;
using System.Collections.Generic;
using System.Linq;
using ConseilOBJ;
using ConseilApp.Builders.Interfaces;
using ConseilBLL.Interfaces;
using ConseilApp.Models.Habillage;
using ConseilApp.Models.Photo;

namespace ConseilApp.Builders
{
    /// <summary>
    /// ici sera fait le mapping / la transformation des données du service en flux Json ou autre
    /// </summary>
    public class HabillageBuilder : IHabillageBuilder
    {
        private IHabillageService _HabillageService;
        private IPhotoService _PhotoService;
        private IConseilService _ConseilService;
        private IVetementService _VetementService;
        private ITypeVetementService _TypeVetementService;

        public HabillageBuilder(IHabillageService HabillageService, IPhotoService PhotoService,
            IConseilService ConseilService, IVetementService VetementService, ITypeVetementService TypeVetementService)
        {
            this._HabillageService = HabillageService;
            this._PhotoService = PhotoService;
            this._ConseilService = ConseilService;
            this._VetementService = VetementService;
            this._TypeVetementService = TypeVetementService;
        }

        // [private] retourne liste des status de conseil selon si c'est un demandeur ou un conseiller
        private List<int> RecupereListeStatutConseil(bool estDemandeur = true)
        {
            List<int> result = new List<int>();

            result.Add((int)DemandeStatus.Accepte);
            if (estDemandeur) { result.Add((int)DemandeStatus.Termine); }

            return result;
        }

        // [public] appelle le service Conseil pour récupérer les conseils selon le style, 
        // la liste des status et l'id de la personne retourne un ModelView
        public IEnumerable<ConseilItemModel> RecupereConseil(int style, int personne, bool estDemandeur)
        {
            List<ConseilItemModel> result = new List<ConseilItemModel>();

            List<int> listStatus = this.RecupereListeStatutConseil(estDemandeur);
            Dictionary<int, string[]> serviceData = estDemandeur ?
                this._ConseilService.RecupereConseilsDemandeurParStatutStyle(listStatus, style, personne) :
                this._ConseilService.RecupereConseilsConseillerParStatutStyle(listStatus, style, personne);

            if (serviceData != null && serviceData.Count > 0)
            {
                // Conseil Id | Demandeur Id | Pseudo | Date création | Nb habillage du conseil
                foreach (var key in serviceData.Keys)
                {
                    result.Add(new ConseilItemModel(key, Convert.ToInt32(serviceData[key][0]), serviceData[key][1], serviceData[key][2], Convert.ToInt32(serviceData[key][3])));
                }
            }

            return result;
        }

        // [public] appelle le service Habillage pour récupérer la liste des habillages 
        // (identifiant, date création et si il ya une note) d'un conseil List<int,string>
        public IDictionary<int, string[]> RecupereConseilHabillages(int conseil)
        {
            Dictionary<int, string[]> result = new Dictionary<int, string[]>();

            var listHabillage = this._HabillageService.RecupereHabillagePourConseil(conseil);
            if (listHabillage != null && listHabillage.Count > 0)
            {
                // Du plus récent au plus ancien
                listHabillage.OrderByDescending(h => h.DateCreation).ToList().ForEach(h => result.Add(h.Id, new string[2] { h.DateCreation.ToShortDateString(),
                    h.Note.HasValue.ToString().ToLower() }));
            }

            return result;
        }

        // [public] appelle le service Photo pour récupérer la liste des photos d'un 
        // habillage et le type de vêtement associé
        public IEnumerable<PhotoViewModel> RecupereHabillagePhotos(int habillage)
        {
            var lst = this._PhotoService.RecuperePhotosPourHabillage(habillage).Select(p => p);

            if (lst != null && lst.Count() > 0) {
                List<PhotoViewModel> result = new List<PhotoViewModel>();
                lst.ToList().ForEach(a => result.Add(new PhotoViewModel(a.Id, a.Url, a.Vetement.TypeId, a.Vetement.Nom)));
                return result;
            }
            else { return new List<PhotoViewModel>(); }
        }

        // [public] appelle le service Photo pour récupérer la liste des photos d'un
        // abonné pour un style
        public IEnumerable<PhotoViewModel> RecupereAbonnePhotoVetementDispoParStyle(int style, int personne)
        {
            var lst = this._PhotoService.RecuperePhotosPourPersonneStyle(personne, PhotoType.Vetement, style);

            if (lst != null && lst.Count() > 0)
            {
                List<PhotoViewModel> result = new List<PhotoViewModel>();
                lst.ToList().ForEach(a => result.Add(new PhotoViewModel(a.Id, a.Url, a.Vetement.TypeId, a.Vetement.Nom)));
                return result;
            }
            else { return new List<PhotoViewModel>(); }
        }

        // [public] appelle le service Photo pour récupérer la liste des photos de vêtement 
        // d'un abonné pour un style et un type de vêtement
        public IEnumerable<PhotoViewModel> RecupereAbonnePhotoVetementDispo(int abonne, int style, int typeVetement)
        {
            var lst = this._PhotoService.RecuperePhotosPourPersonneStyleTypeVetement(abonne, style, typeVetement).Select(p => p);

            if (lst != null && lst.Count() > 0) {
                List<PhotoViewModel> result = new List<PhotoViewModel>();
                lst.ToList().ForEach(a => result.Add(new PhotoViewModel(a.Id, a.Url, a.Vetement.TypeId, a.Vetement.Nom)));
                return result;
            }
            else { return new List<PhotoViewModel>(); }
        }

        // [public] appelle le service Vetement pour récupérer la liste des vêtements
        public IEnumerable<System.Web.Mvc.SelectListItem> RecupereListeVetements()
        {
            return DropDownListBuilder<DropDownListeVetement>.CreateDropDownList(this._VetementService.RecupereListeDesVetementsPourDDL());
        }

        // [public] appelle le service TypeVetement pour récupérer la liste des typs de vêtement
        public IEnumerable<System.Web.Mvc.SelectListItem> RecupereListeTypesVetement()
        {
            return DropDownListBuilder<DropDownListeTypeParam>.CreateDropDownList(this._TypeVetementService.RecupereListeDesTypesVetementPourDDL());
        }

        // [public] Cloturer un conseil
        public void ConseilTermine(int conseil)
        {
            this._ConseilService.TerminerConseil(conseil);
        }

        // [public] Valider un habillage : il faut obligatoirement 3 types de vêtement : Buste, Jambe, Pied !!
        public int HabillageValide(int conseil, List<int> photoIds)
        {
            // Vérifie si dans la liste de photo il y a les 3 type de vêtement de base d'un habillage :
            bool EstHabillageValide = _PhotoService.EstHabillageValide(photoIds);

            if (!EstHabillageValide) return -1; // les trois vêtements de base n'ont pas été respecté

            // A - créer l'enregistrement de l'habillage (avec le conseilId)
            int habillageId = _HabillageService.SauvegardeHabillage(null, conseil, DateTime.Now, null);

            // B - associe les photos de vêtement à l'habillage créé
            int nbPhoto = _HabillageService.SauvegardePhotosHabillage(habillageId, photoIds);

            return nbPhoto;
        }

        // [public] Supprimer un habillage
        public bool HabillageSupprime(int habillage)
        {
            return _HabillageService.SupprimeHabillage(habillage);
        }

        // [public] Soumettre une note au conseil
        public void ConseilValide(int habillage, int conseil, Int16 note)
        {
            this._HabillageService.SauvegardeHabillage(habillage, conseil, DateTime.Now, note);
        }
    }
}
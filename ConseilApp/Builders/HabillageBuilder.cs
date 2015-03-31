using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ConseilOBJ;
using ConseilApp.Builders.Interfaces;
using ConseilBLL.Interfaces;
using ConseilApp.Models.Habillage;

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

        public HabillageBuilder(IHabillageService HabillageService, IPhotoService PhotoService, IConseilService ConseilService, IVetementService VetementService)
        {
            this._HabillageService = HabillageService;
            this._PhotoService = PhotoService;
            this._ConseilService = ConseilService;
            this._VetementService = VetementService;
        }

        // [private] retourne liste des status de conseil selon si c'est un demandeur ou un conseiller
        private List<int> RecupereListeStatutConseil(bool estDemandeur = true) {
            List<int> result = new List<int>();

            result.Add((int)DemandeStatus.Accepte);
            if (estDemandeur) { result.Add((int)DemandeStatus.Termine); }

            return result;
        }

        // [public] appelle le service Conseil pour récupérer les conseils selon le style, la liste des status et l'id de la personne retourne un ModelView
        public IEnumerable<ConseilItem> RecupereConseil(int style, int personne, bool estDemandeur)
        {
            List<ConseilItem> result = new List<ConseilItem>();

            List<int> listStatus = this.RecupereListeStatutConseil(estDemandeur);
            Dictionary<int, string[]> serviceData = estDemandeur ?
                this._ConseilService.RecupereConseilsDemandeurParStatutStyle(listStatus, style, personne) :
                this._ConseilService.RecupereConseilsConseillerParStatutStyle(listStatus, style, personne);

            if (serviceData != null && serviceData.Count > 0) {
                // Conseil Id | Demandeur Id | Pseudo | Date création | Nb habillage du conseil
                foreach (var key in serviceData.Keys) {
                    result.Add(new ConseilItem(key, Convert.ToInt32(serviceData[key][0]), serviceData[key][1], serviceData[key][2], Convert.ToInt32(serviceData[key][3])));
                }
            }

            return result;
        }

        // [public] appelle le service Habillage pour récupérer la liste des habillages d'un conseil List<int,string>
        public IDictionary<int, string> RecupereConseilHabillages(int conseil)
        {
            Dictionary<int, string> result = new Dictionary<int, string>();

            var listHabillage = this._HabillageService.RecupereHabillagePourConseil(conseil);
            if (listHabillage != null && listHabillage.Count > 0) {
                // Du plus récent au plus ancien
                listHabillage.OrderByDescending(h => h.DateCreation).ToList().ForEach(h => result.Add(h.Id, h.DateCreation.ToShortDateString()));
            }

            return result;
        }

        // [public] appelle le service Photo pour récupérer la liste des photos d'un habillage
        public IList<string> RecupereHabillagePhotos(int habillage)
        {
            var listPhotos = this._PhotoService.RecuperePhotosPourHabillage(habillage).Select(p => p.Url).ToList();
            if (listPhotos != null && listPhotos.Count > 0) return listPhotos;
            else return new List<string>();
        }

        // [public] appelle le service Photo pour récupérer la liste des photos selon le type de vêtement et le style en cours
        public IEnumerable<System.Web.Mvc.SelectListItem> RecupereListeVetements()
        {
            return DropDownListBuilder<DropDownListeVetement>.CreateDropDownList(this._VetementService.RecupereListeDesVetementsPourDDL());
        }

        // [public] Cloturer un conseil
        public void ConseilTermine(int conseil)
        {
            this._ConseilService.TerminerConseil(conseil);
        }

        // [public] Valider un habillage
        //int SauvegardeHabillage(int? habillageId, int conseilId, System.DateTime jour, short? note)
        public int HabillageValide()
        {
            // A - Vérifie que les photos pour les types de vêtement obligatoire soient dans la liste

            // B - Upload les photos

            // C - Si toutes les photos ont été téléchargé alors 

            // C1 - créer l'enregistrement de l'habillage

            // C2 - associe les photos de vêtement à l'habillage créé

            // C3 - associe l'habillage créé au conseil
            
            return 0;
        }

        // [public] Soumettre une validation de conseil, avec la note



    }
}
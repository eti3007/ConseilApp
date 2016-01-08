using System;
using System.Collections.Generic;
using ConseilApp.Models.Habillage;
using ConseilApp.Models.Photo;

namespace ConseilApp.Builders.Interfaces
{
    public interface IHabillageBuilder
    {
        IEnumerable<ConseilItemModel> RecupereConseil(int style, int personne, bool estDemandeur);
        IDictionary<int, string[]> RecupereConseilHabillages(int conseil);
        IEnumerable<PhotoViewModel> RecupereHabillagePhotos(int habillage);
        IEnumerable<PhotoViewModel> RecupereAbonnePhotoVetementDispo(int abonne, int style, int typeVetement);
        IEnumerable<PhotoViewModel> RecupereAbonnePhotoVetementDispoParStyle(int style, int personne);
        IEnumerable<System.Web.Mvc.SelectListItem> RecupereListeVetements();
        IEnumerable<System.Web.Mvc.SelectListItem> RecupereListeTypesVetement();
        void ConseilTermine(int conseil);
        int HabillageValide(int conseil, List<int> photoIds);
        bool HabillageSupprime(int habillage);
        void ConseilValide(int habillage, int conseil, Int16 note);
    }
}

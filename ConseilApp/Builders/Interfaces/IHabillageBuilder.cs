using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConseilApp.Models.Habillage;

namespace ConseilApp.Builders.Interfaces
{
    public interface IHabillageBuilder
    {
        IEnumerable<ConseilItem> RecupereConseil(int style, int personne, bool estDemandeur);
        IDictionary<int, string> RecupereConseilHabillages(int conseil);
        IList<string> RecupereHabillagePhotos(int habillage);
        IEnumerable<System.Web.Mvc.SelectListItem> RecupereListeVetements();
        void ConseilTermine(int conseil);
        int HabillageValide(int conseil, List<int> photoIds);
        void ConseilValide(int habillage, int conseil, Int16 note);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConseilREP.Interfaces;
using ConseilBLL.Interfaces;
using ConseilOBJ;

namespace ConseilBLL
{
    public class StyleService : IStyleService
    {
        IStyleRepository repository;

        public StyleService(IStyleRepository repo)
        {
            repository = repo;
        }

        public List<Style> RecupereListeDesStyles()
        {
            return repository.GetList();
        }

        public List<Style> RecupereListeDesStylesConseiller(int personneId)
        {
            return repository.GetListForHabillage(personneId);
        }

        public List<DropDownListeStyle> RecupereListeDesStylesPourDDL()
        {
            List<DropDownListeStyle> result = new List<DropDownListeStyle>();
            var lst = repository.GetList();
            if (lst != null)
                foreach (var item in lst) {
                    result.Add(new DropDownListeStyle() { Id = item.Id, Nom = item.Nom });
                }
            return result;
        }

        public List<DropDownListeStyle> RecupereListeDesStylesConseillerPourDDL(int personneId)
        {
            List<DropDownListeStyle> result = new List<DropDownListeStyle>();
            var lst = repository.GetListForHabillage(personneId);
            if (lst != null)
                foreach (var item in lst) {
                    result.Add(new DropDownListeStyle() { Id = item.Id, Nom = item.Nom });
                }
            return result;
        }
    }
}

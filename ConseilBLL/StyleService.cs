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

        public List<DropDownListeStyle> RecupereListeDesStylesPourDDL()
        {
            List<DropDownListeStyle> result = new List<DropDownListeStyle>();
            foreach (var item in repository.GetList())
            {
                result.Add(new DropDownListeStyle() { Id = item.Id, Nom = item.Nom });
            }
            return result;
        }
    }
}

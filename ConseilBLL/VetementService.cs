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
    public class VetementService : IVetementService
    {
        IVetementRepository repository;

        public VetementService(IVetementRepository repo)
        {
            repository = repo;
        }

        public List<Vetement> RecupereListeDesVetements()
        {
            return repository.GetList();
        }

        public List<DropDownListeVetement> RecupereListeDesVetementsPourDDL()
        {
            List<DropDownListeVetement> result = new List<DropDownListeVetement>();
            foreach (var item in repository.GetList())
            {
                result.Add(new DropDownListeVetement() { Id = item.Id, Nom = item.Nom });
            }
            return result;
        }
    }
}

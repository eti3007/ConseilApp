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
    public class TypeVetementService : ITypeVetementService
    {
        ITypeVetementRepository repository;

        public TypeVetementService(ITypeVetementRepository repo)
        {
            repository = repo;
        }

        public List<TypeParam> RecupereListeDesTypesVetement()
        {
            return repository.GetList();
        }

        public List<DropDownListeTypeParam> RecupereListeDesTypesVetementPourDDL()
        {
            List<DropDownListeTypeParam> result = new List<DropDownListeTypeParam>();
            foreach (var item in repository.GetList())
            {
                result.Add(new DropDownListeTypeParam() { Id = item.Id, Nom = item.ParamLib });
            }
            return result;
        }
    }
}

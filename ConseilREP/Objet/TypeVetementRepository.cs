using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Validation;
using System.Data.Entity;
using ConseilOBJ;
using ConseilDAL;
using ConseilDAL.Exceptions;
using ConseilREP.Interfaces;

namespace ConseilREP
{
    public class TypeVetementRepository : ITypeVetementRepository, IDisposable
    {
        /// <summary>
        /// Récupère la liste des types de vêtement
        /// </summary>
        /// <returns></returns>
        public List<TypeParam> GetList()
        {
            List<TypeParam> result = null;

            using (var context = new ConseilEntitiesBis())
            {
                try
                {
                    result = context.TypeParams.Where(c => c.TypeId == 5).ToList();
                }
                catch (Exception ex)
                {
                    throw new CustomException().CustomGetException(ex);
                }
            }
            return result;
        }

        public void Dispose()
        {
        }
    }
}

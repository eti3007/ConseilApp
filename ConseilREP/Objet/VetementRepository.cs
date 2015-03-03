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
    public class VetementRepository : IVetementRepository, IDisposable
    {
        /// <summary>
        /// Récupère la liste des vêtements
        /// </summary>
        public List<Vetement> GetList()
        {
            List<Vetement> result = null;

            using (var context = new ConseilEntitiesBis())
            {
                try
                {
                    result = context.Vetements.ToList();
                }
                catch (Exception ex)
                {
                    throw new CustomException().CustomGetException(ex, "VetementRepository.GetList");
                }
            }

            return result;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {

        }
    }
}

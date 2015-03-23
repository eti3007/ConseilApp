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

namespace ConseilREP.Objet
{
    public class HabillageRepository : IHabillageRepository, IDisposable
    {
        /// <summary>
        /// Retourne la liste des habillages d'un conseil
        /// </summary>
        /// <param name="id">identifiant du conseil</param>
        /// <returns>liste d'habillage</returns>
        public List<Habillage> GetByConseilId(int id)
        {
            using (var context = new ConseilEntitiesBis()) {
                return context.Habillages.AsQueryable().Where(h => h.ConseilId.Equals(id)).ToList();
            }
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

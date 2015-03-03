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
    public class StyleRepository : IStyleRepository, IDisposable
    {
        /// <summary>
        /// Récupère la liste des styles disponibles
        /// </summary>
        public List<Style> GetList()
        {
            List<Style> result = null;

            using (var context = new ConseilEntitiesBis())
            {
                try
                {
                    result = context.Styles.AsQueryable().ToList();
                }
                catch (Exception ex)
                {
                    throw new CustomException().CustomGetException(ex, "StyleRepository.GetList");
                }
            }

            return result;
        }

        public Style GetById(int styleId)
        {
            using (var context = new ConseilEntitiesBis())
            {
                return context.Styles.AsQueryable().Where(c => c.Id == styleId).FirstOrDefault();
            }
        }

        /// <summary>
        /// Récupère les styles dans lesquelles la personne est conseillère
        /// </summary>
        public List<Style> GetListForHabillage(int persId)
        {
            List<Style> result = null;

            using (var context = new ConseilEntitiesBis())
            {
                result = (from sh in context.StatutHistoriques
                          join s in context.Styles on sh.StyleId equals s.Id
                          where sh.TypeId.Equals(3) &&
                                sh.PersonneId.Equals(persId)
                          select s).ToList();

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

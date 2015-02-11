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
                    throw new CustomException().CustomGetException(ex);
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


        public void Dispose()
        {
        }
    }
}

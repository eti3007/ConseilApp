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
    public class StatutHistoriqueRepository : IStatutHistoriqueRepository
    {
        /// <summary>
        /// Récupère le statut d'une personne pour un style
        /// </summary>
        public int GetPersonStyleStatus(int personneId, int styleId)
        {
            int result = 0;

            using (var context = new ConseilEntitiesBis())
            {
                try
                {
                    result = context.StatutHistoriques.Where(c => (c.PersonneId == personneId) && 
                                                                  (c.StyleId == styleId))
                                                      .Select(c => c.TypeId).FirstOrDefault();
                }
                catch (DbEntityValidationException ex)
                {
                    throw new CustomException().CustomValidationExceptionReturn(ex);
                }
            }
            return result;
        }

        /// <summary>
        /// Met à jour le statut d'une personne pour un style particulier
        /// </summary>
        public void UpdateStatutByStyle(int personneId, int styleId, bool enAttente)
        {
            using (var context = new ConseilEntitiesBis())
            {
                try { 
                    // récupère l'enregistrement concerné :
                    StatutHistorique objStatutHisto = context.StatutHistoriques.Where(s => s.PersonneId.Equals(personneId) && s.StyleId.Equals(styleId)).FirstOrDefault();

                    if (objStatutHisto == null) { return; }

                    // applique la modification du statut (TypeId)
                    objStatutHisto.TypeId = enAttente ? (int)PersonneStatus.EnAttente : (int)PersonneStatus.Abonne;

                    // sauvegarde
                    context.Entry(objStatutHisto).State = EntityState.Modified;
                    context.SaveChanges();                
                }
                catch (DbEntityValidationException ex)
                {
                    throw new CustomException().CustomValidationExceptionReturn(ex);
                }
            }
        }
    }
}

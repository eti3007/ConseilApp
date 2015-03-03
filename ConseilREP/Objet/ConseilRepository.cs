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
    public class ConseilRepository : IConseilRepository, IDisposable
    {
        /// <summary>
        /// envoyer une demande d'aide à un conseiller
        /// </summary>
        /// <param name="consultantId">personne qui reçoit la demande d'aide</param>
        /// <param name="personneId">personne qui fait la demande</param>
        /// <returns>succès/echec</returns>
        public bool AddDressingDemand(int styleId, int consultantId, int personneId)
        {
            bool result = true;
            Conseil conseil = null;

            using (var context = new ConseilEntitiesBis())
            {
                try
                {
                    // ajoute la demande dans la table Conseil
                    conseil = new Conseil()
                    {
                        ConseillerId = consultantId,
                        DemandeurId = personneId,
                        DateCreation = DateTime.Now,
                        TypeId = (int)DemandeStatus.AttenteConseiller,
                        StyleId = styleId,
                        DateLimite = DateTime.Now.AddDays(7)
                    };

                    context.Entry(conseil).State = EntityState.Added;
                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    throw new CustomException().CustomValidationExceptionReturn(ex);
                }
                catch
                {
                    return false;
                }
            }

            // ajoute une notification si l'enregistrement a bien été ajouté
            try
            {
                if (conseil != null)
                {
                    using (var notification = new NotificationRepository())
                    {
                        notification.Notify(styleId, consultantId, personneId, conseil.Id, NotifType.DemandCreation);
                    }
                }
            }
            catch
            {
                // rien à faire...
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

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
        /// ajoute une demande ou une proposition d'aide
        /// Dans la BLL il faut passer le bon type de notifiation par rapport à la vue sur laquelle se trouve l'abonné : Mes demandes ou Mes propositions
        /// </summary>
        public bool AddDressingDemand(int styleId, int conseillerId, int demandeurId, int vTypeId, int typeNotification)
        {
            bool result = true;
            Conseil conseil = null;

            #region CONSEIL
            using (var context = new ConseilEntitiesBis()) {
                try {
                    // ajoute la demande dans la table Conseil
                    conseil = new Conseil() {
                        ConseillerId = conseillerId,
                        DemandeurId = demandeurId,
                        DateCreation = DateTime.Now,
                        TypeId = vTypeId,
                        StyleId = styleId,
                        DateLimite = DateTime.Now.AddDays(7)
                    };

                    context.Entry(conseil).State = EntityState.Added;
                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex) {
                    throw new CustomException().CustomValidationExceptionReturn(ex);
                }
                catch {
                    return false;
                }
            }
            #endregion
            #region NOTIFICATION
            // ajoute une notification si l'enregistrement a bien été ajouté
            try {
                if (conseil != null) {
                    using (var notification = new NotificationRepository()) {
                        notification.Notify(styleId, conseillerId, demandeurId, conseil.Id, (NotifType)typeNotification); //NotifType.DemandCreation);
                    }
                }
            }
            catch {
                // rien à faire...
            }
            #endregion

            return result;
        }

        /// <summary>
        /// maj une demande ou une proposition d'aide avec l'information acceptée ou refusée
        /// Dans la BLL il faut récupérer le type de notification appliquée lors de la création du conseil pour savoir lequel passer en
        /// paramètre dans le cas d'une acceptation ou d'un refus
        /// </summary>
        public bool UpdDressingDemand(int conseilId, int styleId, int conseillerId, int demandeurId, int vTypeId, int typeNotification)
        {
            bool result = true;

            #region CONSEIL
            using (var context = new ConseilEntitiesBis()) {
                try {
                    var conseil = context.Conseils.Where(c => c.Id.Equals(conseilId) &&
                                                              c.DemandeurId.Equals(demandeurId) &&
                                                              c.ConseillerId.Equals(conseillerId) &&
                                                              c.StyleId.Equals(styleId)).FirstOrDefault();
                    if (conseil != null) { conseil.TypeId = vTypeId; }

                    context.Entry(conseil).State = EntityState.Modified;
                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex) {
                    throw new CustomException().CustomValidationExceptionReturn(ex);
                }
                catch {
                    return false;
                }
            }
            #endregion

            #region NOTIFICATION
            // ajoute une notification si la demande/proposition d'aide a été acceptée ou refusée
            try {
                if (conseilId > 0) {
                    using (var notification = new NotificationRepository()) {
                        notification.Notify(styleId, conseillerId, demandeurId, conseilId, (NotifType)typeNotification);
                    }
                }
            }
            catch {
                // rien à faire...
            }
            #endregion

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

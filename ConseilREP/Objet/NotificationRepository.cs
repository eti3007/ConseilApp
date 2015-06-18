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
    public class NotificationRepository : INotificationRepository, IDisposable
    {
        /// <summary>
        /// Ajoute une ou plusieurs notifications pour un conseil selon l'étape d'une demande ou d'une proposition
        /// </summary>
        public bool Notify(int styleId, int consultantId, int personneId, int conseilId, NotifType type)
        {
            bool result = false;
            switch (type)
            {
                case NotifType.DemandCreation: // {0} vous demande de l'aide pour le style {1}       Votre demande a été envoyé à {0}
                    result = SendTwoNotifications(styleId, consultantId, personneId, conseilId, ConstantMessages.MSG_ENVOI_DEMANDE_CONSEILLER,
                                    ConstantMessages.MSG_ENVOI_DEMANDE_DEMANDEUR);
                    break;
                case NotifType.PropositionAccept: // {0} a accepté votre aide pour le style {1}
                    result = this.SendOneNotification(styleId, consultantId, conseilId, ConstantMessages.MSG_ACCEPTE_PROPOSITION_CONSEILLER);
                    break;
                case NotifType.PropositionReject: // {0} a refusé votre aide pour le style {1}
                    result = this.SendOneNotification(styleId, consultantId, conseilId, ConstantMessages.MSG_REFUSE_PROPOSITION_CONSEILLER);
                    break;
                case NotifType.PropositionCreation: // {0} vous propose son aide pour le style {1}       Votre proposition d'aide a été envoyé à {0}
                    result = SendTwoNotifications(styleId, consultantId, personneId, conseilId, ConstantMessages.MSG_ENVOI_PROPOSITION_DEMANDEUR,
                                    ConstantMessages.MSG_ENVOI_PROPOSITION_CONSEILLER);
                    break;
                case NotifType.DemandAccept: // {0} a accepté de vous aider pour le style {1}
                    result = this.SendOneNotification(styleId, personneId, conseilId, ConstantMessages.MSG_ACCEPTE_DEMANDE_DEMANDEUR);
                    break;
                case NotifType.DemandReject: // {0} a refusé de vous aider pour le style {1}
                    result = this.SendOneNotification(styleId, personneId, conseilId, ConstantMessages.MSG_REFUSE_DEMANDE_DEMANDEUR);
                    break;
                case NotifType.Termine: // {0} vous invite à consulter ses propositions d'habillages
                    result = this.SendOneNotification(styleId, personneId, conseilId, ConstantMessages.MSG_CONSEIL_TERMINE);
                    break;
            };
            return result;
        }

        /// <summary>
        /// Liste des notifications (Message) dont le TypeId==7 du plus récent au plus ancien pour un style
        /// et selon si l'utilisateur est demandeur ou conseiller
        /// </summary>
        public List<Notification> GetNotificationsByPersonne(int styleId, int personneId, bool isDemandeur)
        {
            List<Notification> result = new List<Notification>();
            using (var context = new ConseilEntitiesBis())
            {
                if (isDemandeur)
                result = (from n in context.Notifications
                          join c in context.Conseils on n.ConseilId equals c.Id
                          where c.StyleId == styleId && c.DemandeurId == personneId
                             && n.PersonneId == personneId
                          orderby n.DateCreation descending
                          select n
                         ).ToList();
                else
                result = (from n in context.Notifications
                          join c in context.Conseils on n.ConseilId equals c.Id
                          where c.StyleId == styleId && c.ConseillerId == personneId
                             && n.PersonneId == personneId
                          orderby n.DateCreation descending
                          select n
                         ).ToList();

            }
            return result;
        }

        /// <summary>
        /// Ajoute le message de la personne (demandeur ou conseiller) au conseil en cours
        /// </summary>
        public bool SendCustomerMessage(int conseilId, int personneId, string message)
        {
            bool result = false;
            using (var context = new ConseilEntitiesBis())
            {
                try
                {
                    Notification notifConsultant = new Notification()
                    {
                        DateCreation = DateTime.Now,
                        ConseilId = conseilId,
                        PersonneId = personneId,
                        TypeId = (int)MessageType.Notification,
                        Message = message
                    };
                    this.AddNotification(context, ref notifConsultant);
                }
                catch (DbEntityValidationException ex)
                {
                    throw ex;
                }
                catch
                {
                    return false;
                }
            }

            return result;
        }

        private bool SendOneNotification(int styleId, int personneId, int conseilId, string message)
        {
            bool result = false;

            using (var context = new ConseilEntitiesBis())
            {
                try
                {
                    // récupère le nom du style en cours :
                    string styleNom = context.Styles.AsQueryable().Where(c => c.Id == styleId).FirstOrDefault().Nom;

                    // récupère le pseudo des deux personnes
                    string pseudo = context.Personnes.AsQueryable().Where(c => c.Id == personneId).FirstOrDefault().Pseudo;

                    Notification notif = new Notification()
                    {
                        DateCreation = DateTime.Now,
                        ConseilId = conseilId,
                        PersonneId = personneId,
                        TypeId = (int)MessageType.Notification,
                        Message = string.Format(message, pseudo, styleNom)
                    };
                    this.AddNotification(context, ref notif);

                    if (notif.Id > 0) result = true;

                }
                catch (DbEntityValidationException ex)
                {
                    throw ex;
                }
                catch
                {
                    return false;
                }
            }

            return result;
        }

        private bool SendTwoNotifications(int styleId, int consultantId, int personneId, int conseilId, string msgA, string msgB)
        {
            bool result = false;
            using (var context = new ConseilEntitiesBis())
            {
                try
                {
                    DateTime jour = DateTime.Now;
                    // récupère le nom du style en cours :
                    string styleNom = context.Styles.AsQueryable().Where(c => c.Id == styleId).FirstOrDefault().Nom;

                    // récupère le pseudo des deux personnes
                    string pseudoA = context.Personnes.AsQueryable().Where(c => c.Id == consultantId).FirstOrDefault().Pseudo;
                    string pseudoB = context.Personnes.AsQueryable().Where(c => c.Id == personneId).FirstOrDefault().Pseudo;
                    
                    Notification notifConsultant = new Notification()
                    {
                        DateCreation = jour,
                        ConseilId = conseilId,
                        PersonneId = consultantId,
                        TypeId = (int)MessageType.Notification,
                        Message = string.Format(msgA, pseudoA, styleNom)
                    };
                    this.AddNotification(context, ref notifConsultant);

                    if (notifConsultant.Id > 0)
                    {
                        Notification notifDemandeur = new Notification()
                        {
                            DateCreation = jour,
                            ConseilId = conseilId,
                            PersonneId = personneId,
                            TypeId = (int)MessageType.Notification,
                            Message = string.Format(msgB, pseudoB)
                        };
                        this.AddNotification(context, ref notifDemandeur);

                        if (notifDemandeur.Id > 0) result = true;
                    }
                }
                catch (DbEntityValidationException ex)
                {
                    throw ex;
                }
                catch
                {
                    return false;
                }
            }

            return result;
        }

        /// <summary>
        /// Ajoute dans la base la notification
        /// </summary>
        private bool AddNotification(ConseilEntitiesBis context, ref Notification notification)
        {
            bool result = true;
            
            try
            {
                context.Entry(notification).State = EntityState.Added;
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

using System.Collections.Generic;
using ConseilREP.Interfaces;
using ConseilBLL.Interfaces;
using ConseilOBJ;

namespace ConseilBLL
{
    public class NotificationService : INotificationService
    {
        INotificationRepository _NotificationRepository;

        public NotificationService(INotificationRepository NotificationRepository)
        {
            this._NotificationRepository = NotificationRepository;
        }

        public List<Notification> RecupereListeNotification(int styleId, int personneId)
        {
            return this._NotificationRepository.GetNotificationsByPersonne(styleId, personneId);
        }

        public bool PersonneEnvoiMessage(int conseilId, int personneId, string message)
        {
            return this._NotificationRepository.SendCustomerMessage(conseilId, personneId, message);
        }
    }
}

using ConseilOBJ;
using System.Collections.Generic;

namespace ConseilREP.Interfaces
{
    public interface INotificationRepository
    {
        bool Notify(int styleId, int consultantId, int personneId, int conseilId, NotifType type);
        List<Notification> GetNotificationsByPersonne(int styleId, int personneId, bool isDemandeur);
        bool SendCustomerMessage(int conseilId, int personneId, string message);
    }
}

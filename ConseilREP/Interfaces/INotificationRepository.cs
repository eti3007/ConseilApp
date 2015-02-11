using ConseilOBJ;

namespace ConseilREP.Interfaces
{
    public interface INotificationRepository
    {
        bool Notify(int styleId, int consultantId, int personneId, int conseilId, NotifType type);
    }
}

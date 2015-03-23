using ConseilOBJ;
using System;
using System.Collections.Generic;

namespace ConseilREP.Interfaces
{
    public interface IConseilRepository
    {
        bool AddDressingDemand(int styleId, int conseillerId, int demandeurId, int statut, int typeNotification);
        bool UpdDressingDemand(int conseilId, int styleId, int conseillerId, int demandeurId, int statut, int typeNotification);
        Conseil GetById(int conseilId);
        List<Conseil> GetByStatusesStylePerson(List<int> statuses, int style, int personneId, bool demandeur = true);
        bool ExistConseilByIds(int styleId, int conseillerId, int demandeurId, int statut);
    }
}

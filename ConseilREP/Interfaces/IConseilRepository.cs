using System;
using System.Collections.Generic;

namespace ConseilREP.Interfaces
{
    public interface IConseilRepository
    {
        bool AddDressingDemand(int styleId, int conseillerId, int demandeurId, int vTypeId, int typeNotification);
        bool UpdDressingDemand(int conseilId, int styleId, int conseillerId, int demandeurId, int vTypeId, int typeNotification);
    }
}

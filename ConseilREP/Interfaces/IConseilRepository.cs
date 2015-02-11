using System;
using System.Collections.Generic;

namespace ConseilREP.Interfaces
{
    public interface IConseilRepository
    {
        bool AddDressingDemand(int styleId, int consultantId, int personneId);
    }
}

using ConseilOBJ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConseilREP.Interfaces
{
    public interface IHabillageRepository
    {
        List<Habillage> GetByConseilId(int id);
    }
}

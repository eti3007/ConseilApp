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
        int SaveHabillage(int? habillageId, int conseilId, DateTime jour, short? note);
        int AddPicsToHabillage(int habillageId, List<int> picsId);
        bool DeleteHabillage(int habillageId);
    }
}

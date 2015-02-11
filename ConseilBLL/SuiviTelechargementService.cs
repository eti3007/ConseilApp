using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConseilREP.Interfaces;
using ConseilBLL.Interfaces;
using ConseilOBJ;

namespace ConseilBLL
{
    public class SuiviTelechargementService : ISuiviTelechargementService
    {
        ISuiviTelechargeRepository repository;

        public SuiviTelechargementService(ISuiviTelechargeRepository repo)
        {
            repository = repo;
        }

        public void ConfigureSuivieTelechargement(DateTime jour, int personneId, bool estNouveau)
        {
            repository.Add(jour, personneId, estNouveau);
        }

        public int NbPhotoTelechargeable(DateTime jour, int personneId)
        {
            int result = 0;

            result = repository.NbPhotoToUpload(jour, personneId);
            
            return result;
        }
    }
}

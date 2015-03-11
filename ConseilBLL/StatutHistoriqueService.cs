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
    public class StatutHistoriqueService : IStatutHistoriqueService
    {
        IStatutHistoriqueRepository repository;

        public StatutHistoriqueService(IStatutHistoriqueRepository repo)
        {
            repository = repo;
        }

        public int RecupereStatusPourPersonneEtStyle(int personneId, int styleId)
        {
            return repository.GetPersonStyleStatus(personneId, styleId);
        }

        public void MajStyleStatutByPersonne(int personneId, int styleId, bool enAttente)
        {
            repository.UpdateStatutByStyle(personneId, styleId, enAttente);
        }
    }
}

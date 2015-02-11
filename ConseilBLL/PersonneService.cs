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
    public class PersonneService : IPersonneService
    {
        IPersonneRepository repository;

        public PersonneService(IPersonneRepository repo)
        {
            repository = repo;
        }

        public int AjouteNouveau(Personne personne, int styleId, string pseudo)
        {
            return repository.AddNew(personne, styleId, pseudo);
        }

        public Personne RecuperePersonneConnecte(string pseudo)
        {
            return repository.GetPersonneLogin(pseudo);
        }

        public List<PersonViewItem> ConseillersAptePourAider(int styleId, int personneId)
        {
            return repository.GetListenersFreeToHelp(styleId, personneId);
        }

        public List<PersonViewItem> ConseillersProposeAide(int styleId, int personneId)
        {
            return repository.GetListenersWantToHelp(styleId, personneId);
        }

        public List<PersonViewItem> AbonnesAttenteAide(int styleId, int personneId)
        {
            return repository.GetSubscibersWaitForHelp(styleId, personneId);
        }

        public List<PersonViewItem> AbonnesDemandeAide(int styleId, int personneId)
        {
            return repository.GetSubscribersAskForHelp(styleId, personneId);
        }
    }
}

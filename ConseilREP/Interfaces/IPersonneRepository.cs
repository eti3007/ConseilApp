using System.Collections.Generic;
using ConseilOBJ;

namespace ConseilREP.Interfaces
{
    public interface IPersonneRepository
    {
        int AddNew(Personne personneToAdd, int styleId, string pseudo);
        List<PersonViewItem> GetListenersFreeToHelp(int styleId, int personneId);
        List<PersonViewItem> GetListenersWantToHelp(int styleId, int personneId);
        List<PersonViewItem> GetSubscibersWaitForHelp(int styleId, int personneId);
        List<PersonViewItem> GetSubscribersAskForHelp(int styleId, int personneId);
        Personne GetPersonneLogin(string login);
    }
}

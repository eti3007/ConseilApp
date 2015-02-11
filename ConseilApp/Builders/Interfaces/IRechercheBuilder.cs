using System.Collections.Generic;
using ConseilOBJ;
using ConseilApp.Models.Recherche;

namespace ConseilApp.Builders.Interfaces
{
    public interface IRechercheBuilder
    {
        void SetPersonneStyleIdentifier(int PersonneId, int StyleId);
        IEnumerable<ApteConseil> DemandeAbonnePeutAider();
        IEnumerable<ProposeAide> DemandeAbonneProposeAider();
        IEnumerable<AttenteConseil> PropositionAbonneAttenteConseil();
        IEnumerable<SolliciteAide> PropositionAbonneSolliciteAide();
    }
}

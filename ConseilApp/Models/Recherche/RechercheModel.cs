using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConseilApp.Models.Recherche
{
    public class RechercheModel
    {
        public PremiereListe premiereListe { get; set; }
        public DeuxiemeListe deuxiemeListe { get; set; }
    }

    public class PremiereListe
    {
        // Demande
        public IEnumerable<ApteConseil> apteConseil { get; set; }
        // Proposition
        public IEnumerable<AttenteConseil> attenteConseil { get; set; }
    }

    public class DeuxiemeListe
    {
        // Demande
        public IEnumerable<ProposeAide> proposeAide { get; set; }
        // Proposition
        public IEnumerable<SolliciteAide> solliciteAide { get; set; }
    }
}
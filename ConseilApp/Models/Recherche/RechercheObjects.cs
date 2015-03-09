using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConseilApp.Models.Recherche
{
    public abstract class BaseConseil
    {
        // Abonné Id
        public int id { get; set; }
        public string pseudo { get; set; }
        public char genre { get; set; }
        public int nbPhotoHabille { get; set; }
    }

    /// <summary>
    /// Concerne la vue _ListeEnAttente => Conseillers qui peuvent aider
    /// </summary>
    public class ApteConseil : BaseConseil
    {
        public int nbDemande { get; set; }
        public string note { get; set; }
        public int nbProposition { get; set; }
    }

    /// <summary>
    /// Concerne la vue _ListeEnSoutien
    /// </summary>
    public class ProposeAide : BaseConseil
    {
        public int nbAide { get; set; }
        public string note { get; set; }
        public int nbHabPropose { get; set; }
    }

    /// <summary>
    /// Concerne la vue _ListeEnAttente => Abonnés en attente de conseil
    /// </summary>
    public class AttenteConseil : BaseConseil
    {
        public int nbPhotoVetement { get; set; }
    }

    /// <summary>
    /// Concerne la vue _ListeEnSoutien
    /// </summary>
    public class SolliciteAide : BaseConseil
    {
        public int nbPhotoVetement { get; set; }
    }

    public class BaseConseils
    {
        public IEnumerable<BaseConseil> baseConseils { get; set; }
        public bool isRechercheDemande { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConseilApp.Models.Recherche
{
    public class RechercheReponseModel
    {
        public string Titre { get; set; }
        //public string Pseudo { get; set; }

        // utilisé par :
        // les abonnés qui vous propose leur aide
        // les abonnés sollicitant votre aide
        public int ConseilId { get; set; }

        public int DemandeurId { get; set; }
        public int ConseillerId { get; set; }
        public int StyleId { get; set; }

        public List<string> UrlValidation { get; set; }
    }
}
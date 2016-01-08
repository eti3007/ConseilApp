using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConseilApp.Models.Habillage
{
    public class ConceptionModel
    {
        public IEnumerable<System.Web.Mvc.SelectListItem> typeVetementListe { get; set; }
        public IEnumerable<Photo.PhotoViewModel> vetements { get; set; }
    }
}
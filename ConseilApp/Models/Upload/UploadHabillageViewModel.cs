using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ConseilApp.Models
{
    public class UploadHabillageViewModel : UploadViewModel
    {
        [Required(ErrorMessage = "Veuillez choisir un style")]
        public string Style { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> styleListe { get; set; }

        public int NombrePhotosMax { get; set; }
    }
}
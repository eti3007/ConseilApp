using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ConseilApp.Models
{
    public class UploadVetementViewModel : UploadViewModel
    {
        public UploadVetementViewModel()
        {
            this.TypePhotoId = (int)ConseilOBJ.PhotoType.Vetement;
        }

        [Required(ErrorMessage = "Veuillez choisir un style")]
        public string Style { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> styleListe { get; set; }

        [Required(ErrorMessage = "Veuillez choisir un type de vêtement")]
        public string Vetement { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> vetementListe { get; set; }

        [Display(Name="Attente de conseil")]
        public bool ModeAttente { get; set; }

        public int NombrePhotosMax { get; set; }
    }
}
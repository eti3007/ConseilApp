using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConseilApp.Models
{
    public class UploadPhotoViewModel
    {
        public UploadVetementViewModel PhotoVetement { get; set; }
        public bool EstConseiller { get; set; }
        public UploadHabillageViewModel PhotoHabillage { get; set; }
        public bool VetementValidation { get; set; }
    }
}
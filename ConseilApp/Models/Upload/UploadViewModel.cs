using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ConseilApp.Models
{
    public class UploadViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int TypePhotoId { get; set; }
        public int NbPhotoMax { get; set; }
        public List<string> UrlListe { get; set; }
    }
}
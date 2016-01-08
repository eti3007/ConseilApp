using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConseilApp.Models.Photo
{
    public class PhotoViewModel
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int TypeVetementId { get; set; }
        public string TypeVetement { get; set; }

        public PhotoViewModel(int Id, string Url, int TypeVetementId, string TypeVetement)
        {
            this.Id = Id;
            this.Url = Url;
            this.TypeVetementId = TypeVetementId;
            this.TypeVetement = TypeVetement;
        }
    }
}
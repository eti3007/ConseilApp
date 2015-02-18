using System.Collections.Generic;
//using System.Linq;
//using System.Web.Mvc;
using ConseilOBJ;
using ConseilApp.Builders.Interfaces;

namespace ConseilApp.Builders
{
    public class PhotoBuilder : IPhotoBuilder
    {
        public List<string> UrlPhotoListe(List<Photo> photos)
        {
            if (photos == null) return new List<string>();

            List<string> objListe = new List<string>();
            string value;

            foreach (var item in photos)
            {
                if (!string.IsNullOrEmpty(item.Url))
                {
                    value = item.Url;
                    objListe.Add(value);
                }
            }

            return objListe;
        }
    }
}
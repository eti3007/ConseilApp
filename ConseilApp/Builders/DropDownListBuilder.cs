using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ConseilOBJ;
using ConseilApp.Builders.Interfaces;

namespace ConseilApp
{
    public static class DropDownListBuilder<T> where T : IDropDownListeObject
    {
        public static List<SelectListItem> CreateDropDownList(List<T> objet)
        {
            List<SelectListItem> objListe = new List<SelectListItem>();
            SelectListItem itemList;

            // alimente la liste des styles
            foreach (var item in objet.OrderBy(x => x.Nom))
            {
                itemList = new SelectListItem();
                itemList.Text = item.Nom;
                itemList.Value = item.Id.ToString();
                objListe.Add(itemList);
            }

            return objListe;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConseilApp.Controllers
{
    public class ListeController : BaseController
    {
        [Authorize]
        public string ListeStyle()
        {
            if (base.ListeDesStyles == null) return "Style > ...";

            System.Text.StringBuilder selection = new System.Text.StringBuilder();
            int styleId = base.StyleEnCours;

            selection.Append("Style > <select id='Style' class='selectNav'>"); // onchange='javascript:ChangeStyleProcess(this)'
            foreach (var obj in base.ListeDesStyles)
            {
                if (styleId == obj.Id) selection.Append("<option value='" + obj.Id.ToString() + "' selected>" + obj.Nom + "</option>");
                else selection.Append("<option value='" + obj.Id.ToString() + "'>" + obj.Nom + "</option>");
            }
            selection.Append("</select>");

            return selection.ToString();
        }

        [Authorize]
        public ActionResult MajStyleEnCours(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                UpdateStyleCookieValue(id);

                return RedirectToAction("Index", "Home");
            }
            return null;
        }
    }
}

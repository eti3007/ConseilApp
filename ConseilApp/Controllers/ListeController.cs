using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ConseilApp.Builders.Interfaces;

namespace ConseilApp.Controllers
{
    public class ListeController : BaseController
    {
        private IMenuBuilder _MenuBuilder;

        public ListeController(IMenuBuilder MenuBuilder) { this._MenuBuilder = MenuBuilder; }

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

        private string DropdownStyle()
        {
            System.Text.StringBuilder selection = new System.Text.StringBuilder();

            selection.Append("<div class=\"row\">");
            selection.Append("<div class=\"col-lg-6\"><div class=\"input-group\"><div class=\"input-group-btn\">");
            selection.Append("<button type=\"button\" class=\"btn btn-default dropdown-toggle\" data-toggle=\"dropdown\" aria-expanded=\"false\"> Styles <span class=\"caret\"></span></button>");
            selection.Append("<ul class=\"dropdown-menu\" role=\"menu\">");

            int styleId = base.StyleEnCours;
            foreach (var obj in base.ListeDesStyles) {
                selection.Append("<li><a href=\"#" + obj.Id.ToString() + "\">" + obj.Nom + "</a></li>");
                //if (styleId == obj.Id) selection.Append("<li><a href=\"#"+ obj.Id.ToString() +"\">"+obj.Nom+"</a></li>");
                //else selection.Append("<option value='" + obj.Id.ToString() + "'>" + obj.Nom + "</option>");
            }
                        
            selection.Append("</ul>");
            selection.Append("</div><!-- /btn-group -->");
            selection.Append("<input type=\"text\" class=\"form-control\" aria-label=\"...\">");
            selection.Append("</div></div></div>");



            return selection.ToString();
        }
    }
}

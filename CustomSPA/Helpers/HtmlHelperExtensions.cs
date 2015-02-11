using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace CustomSPA.Helpers
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString Vignette(this HtmlHelper htmlHelper, string urlPicture, string topText="", string bottomText="")
        {
            var table = new TagBuilder("table");
            var span = new TagBuilder("span");
            var tr = new TagBuilder("tr");
            var td = new TagBuilder("td");
            var img = new TagBuilder("img");

            table.AddCssClass("table-striped");
            table.AddCssClass("table-bordered");
            td.Attributes.Add("align", "center");

            // premier texte
            if (!string.IsNullOrEmpty(topText))
            {
                span.AddCssClass("text-info");
                span.InnerHtml = topText;
                td.InnerHtml = span.ToString();
                tr.InnerHtml = td.ToString();
                table.InnerHtml = tr.ToString();
            }

            // l'image
            img.Attributes.Add("src", urlPicture);
            img.Attributes.Add("style", "width:170px;height:170px");
            img.AddCssClass("img-polaroid");
            td.InnerHtml = img.ToString();
            tr.InnerHtml = td.ToString();
            table.InnerHtml = table.InnerHtml + tr.ToString();

            // deuxième texte
            if (!string.IsNullOrEmpty(topText))
            {
                span = new TagBuilder("span");
                span.InnerHtml = bottomText;
                td.InnerHtml = span.ToString();
                tr.InnerHtml = td.ToString();
                table.InnerHtml = table.InnerHtml + tr.ToString();
            }

            return new MvcHtmlString(table.ToString());
        }
    }
}
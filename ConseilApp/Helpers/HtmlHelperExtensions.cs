using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using ConseilApp.Models;

namespace ConseilApp.Helpers
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString Vignette(this HtmlHelper htmlHelper, string urlPicture, string pathPicture = "", string topText = "", string bottomText = "")
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
                if (!string.IsNullOrEmpty(pathPicture)) td.Attributes.Add("onclick", "SupprimeImage('" + pathPicture + "')");
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
            if (!string.IsNullOrEmpty(bottomText))
            {
                span = new TagBuilder("span");
                span.InnerHtml = bottomText;
                td.InnerHtml = span.ToString();
                tr.InnerHtml = td.ToString();
                table.InnerHtml = table.InnerHtml + tr.ToString();
            }

            return new MvcHtmlString(table.ToString());
        }

        /* PAS UTILISER  => A SUPPRIMER
        public static MvcHtmlString TableauVignette(this HtmlHelper htmlHelper, List<PhotoViewModel> listPhoto)
        {
            var table = new TagBuilder("table");
            var tr = new TagBuilder("tr");
            var td = new TagBuilder("td");
            short i = 0;
            
            foreach (var photo in listPhoto)
            {
                i++; 

                // set td
                td.InnerHtml = Vignette(htmlHelper, photo.Url, photo.Vetement).ToString();

                // add td in tr
                tr.InnerHtml = tr.InnerHtml + td.ToString();

                if (i == 3)
                {
                    // add tr in table
                    table.InnerHtml = tr.ToString();

                    // reset tr
                    tr = new TagBuilder("tr");

                    // reset i
                    i = 0;
                }                
            }

            if (i > 0 && i < 3)
            {
                // add tr in table
                table.InnerHtml = tr.ToString();
            }

            return new MvcHtmlString(table.ToString());
        }
        */
        public static MvcHtmlString ModalShow(this HtmlHelper htmlHelper, string Name, string Title, string Body, bool large=false)
        {
            var divGlobal = new TagBuilder("div");
            var divDialog = new TagBuilder("div"); // class="modal-dialog">
            var divContent = new TagBuilder("div"); //<div class="modal-content">
            var divHeader = new TagBuilder("div");
            var divBody = new TagBuilder("div");
            var divFooter = new TagBuilder("div");
            var btn = new TagBuilder("button");
            var h3 = new TagBuilder("h3");

            divHeader.AddCssClass("modal-header");
            btn.AddCssClass("close");
            btn.Attributes.Add("type", "button");
            btn.Attributes.Add("data-dismiss", "modal");
            btn.Attributes.Add("aria-hidden", "true");
            btn.InnerHtml = "x";
            h3.Attributes.Add("id", "myModalLabel");
            h3.InnerHtml = Title;
            divHeader.InnerHtml = btn.ToString() + h3.ToString();

            divBody.AddCssClass("modal-body");
            divBody.InnerHtml = Body;

            divFooter.AddCssClass("modal-footer");
            btn = new TagBuilder("button");
            btn.AddCssClass("btn");
            btn.Attributes.Add("data-dismiss", "modal");
            btn.Attributes.Add("aria-hidden", "true");
            btn.InnerHtml = "Fermer";
            divFooter.InnerHtml = btn.ToString();

            divContent.AddCssClass("modal-content");
            divContent.InnerHtml = divHeader.ToString() + divBody.ToString() + divFooter.ToString();

            divDialog.AddCssClass("modal-dialog");
            if (large) divDialog.AddCssClass("modal-lg");
            divDialog.InnerHtml = divContent.ToString();

            divGlobal.AddCssClass("modal");
            divGlobal.AddCssClass("hide");
            divGlobal.AddCssClass("fade");
            if (large) divGlobal.AddCssClass("bs-example-modal-lg");
            divGlobal.Attributes.Add("id", Name);
            divGlobal.Attributes.Add("tabindex", "-1");
            divGlobal.Attributes.Add("z-index", "1051");
            divGlobal.Attributes.Add("role", "dialog");
            divGlobal.Attributes.Add("aria-labelledby", "myModalLabel");
            divGlobal.Attributes.Add("aria-hidden", "true");

            divGlobal.InnerHtml = divDialog.ToString();

            return new MvcHtmlString(divGlobal.ToString());
        }

        public static MvcHtmlString LinkButtonForModal(this HtmlHelper htmlHelper, string ModalName, string Text, bool isButton = true)
        {
            var hRef = new TagBuilder("a");

            if (isButton)
            {
                hRef.AddCssClass("btn");
                hRef.Attributes.Add("role", "button");
            }
            hRef.Attributes.Add("href", "#" + ModalName);
            hRef.Attributes.Add("data-toggle", "modal");
            hRef.InnerHtml = Text;

            return new MvcHtmlString(hRef.ToString());
        }

        /* PAS UTILISER  => A SUPPRIMER
        public static MvcHtmlString Carousel(this HtmlHelper htmlHelper, List<PhotoViewModel> list, string Name, int width = 0, int height = 0)
        {
            var divGlobal = new TagBuilder("div");
            var indicators = new TagBuilder("ol");
            var indicator = new TagBuilder("li");
            var items = new TagBuilder("div");
            var item = new TagBuilder("div");
            var img = new TagBuilder("img");
            var caption = new TagBuilder("div");
            var navLeft = new TagBuilder("a");
            var navRight = new TagBuilder("a");

            divGlobal.AddCssClass("carousel");
            divGlobal.AddCssClass("slide");
            divGlobal.Attributes.Add("id", Name);
            indicators.AddCssClass("carousel-indicators");
            items.AddCssClass("carousel-inner");
            caption.AddCssClass("carousel-caption");

            short i = -1;
            foreach (var photo in list)
            {
                i++;

                if (i == 0)
                {
                    indicator.AddCssClass("active");
                    item.AddCssClass("active");
                }

                indicator.Attributes.Add("data-target", "#" + Name);
                indicator.Attributes.Add("data-slide-to", i.ToString());
                indicators.InnerHtml += indicator.ToString();

                item.AddCssClass("item");
                img.Attributes.Add("src", photo.Url);
                caption.InnerHtml = photo.Info;
                item.InnerHtml = img.ToString() + caption.ToString();
                items.InnerHtml += item.ToString();

                // reinit
                indicator = new TagBuilder("li");
                item = new TagBuilder("div");
                img = new TagBuilder("img");
            }

            navLeft.AddCssClass("left");
            navLeft.AddCssClass("carousel-control");
            navLeft.Attributes.Add("data-slide", "prev");
            navLeft.Attributes.Add("href", "#" + Name);
            navLeft.InnerHtml = "<";

            navRight.AddCssClass("right");
            navRight.AddCssClass("carousel-control");
            navRight.Attributes.Add("data-slide", "next");
            navRight.Attributes.Add("href", "#" + Name);
            navRight.InnerHtml = ">";

            if (width > 0 && height > 0)
                divGlobal.Attributes.Add("style", "width:" + width.ToString() + "px;height:" + height.ToString() + "px");
            else
            {
                if (width > 0) divGlobal.Attributes.Add("style", "width:" + width.ToString() + "px");
                if (height > 0) divGlobal.Attributes.Add("style", "height:" + height.ToString() + "px");
            }

            divGlobal.InnerHtml = indicators.ToString() + items.ToString() + navLeft.ToString() + navRight.ToString();

            return new MvcHtmlString(divGlobal.ToString());
        }
        */
        public static MvcHtmlString NotifItem(this HtmlHelper htmlHelper, NotificationViewModel notif)
        {
            var div = new TagBuilder("div");
            var table = new TagBuilder("table");
            var tr = new TagBuilder("tr");
            var td = new TagBuilder("td");
            var span = new TagBuilder("span");
            var p = new TagBuilder("p");

            table.AddCssClass("table-striped");
            table.AddCssClass("table-bordered");

            // initialisation de la ligne
            // récupère la couleur selon le type de notification : 
            switch (notif.TypeNotif)
            {
                // Demandé
                case 24:
                case 29:
                    div.Attributes.Add("style", "background-color:#FFECD1");
                    break;
                // Accepté
                case 25:
                case 27:
                    div.Attributes.Add("style", "background-color:#C5E3C5");
                    break;
                // Rejeté
                case 26:
                case 28:
                    div.Attributes.Add("style", "background-color:#F2BBB8");
                    break;
            }

            // préparation du message de la notification
            span.AddCssClass("text-info");
            span.InnerHtml = notif.Message;

            // préparation de la date de création de la notification
            p.AddCssClass("text-left");
            p.InnerHtml = notif.DateNotif;

            // remplissage de la cellule
            td.InnerHtml = p.ToString() + span.ToString();
            div.InnerHtml = p.ToString() + span.ToString();

            // remplissage de la ligne
            tr.InnerHtml = td.ToString();

            // ajout de la ligne dans le tableau
            table.InnerHtml = table.InnerHtml + tr.ToString();

            return new MvcHtmlString(div.ToString());
        }
    }
}
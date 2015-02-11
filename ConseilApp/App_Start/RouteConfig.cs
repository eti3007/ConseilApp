using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ConseilApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "UploadPhotos",
                url: "{controller}/{action}/{styleId}/{vetementId}",
                defaults: new { controller = "Upload", action = "UploadPhotos", vetementId = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "SetDefaultStyle",
                url: "{controller}/{action}",
                defaults: new { controller = "Liste", action = "MajStyleEnCours" }
            );

            routes.MapRoute(
                name: "MesDemandes",
                url: "{controller}/{action}",
                defaults: new { controller = "Recherche", action = "Demandes" }
            );

            routes.MapRoute(
                name: "MesPropositions",
                url: "{controller}/{action}",
                defaults: new { controller = "Recherche", action = "Propositions" }
            );
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ConseilOBJ;
using ConseilApp.Builders.Interfaces;
using ConseilBLL.Interfaces;
using ConseilApp.Models.Recherche;

namespace ConseilApp.Builders
{
    /// <summary>
    /// ici sera fait le mapping / la transformation des données du service en flux Json ou autre
    /// </summary>
    public class HabillageBuilder : IHabillageBuilder
    {
        private IHabillageService _HabillageService;
        private IPhotoService _PhotoService;
        private IConseilService _ConseilService;
        private int _StyleId;
        private int _PersonneId;

        public HabillageBuilder(IHabillageService HabillageService, IPhotoService PhotoService)
        {
            this._HabillageService = HabillageService;
            this._PhotoService = PhotoService;
        }

        // [private] retourne liste des status de conseil selon si c'est un demandeur ou un conseiller

        // [public] appelle le service Conseil pour récupérer les conseils selon le style, la liste des status et l'id de la personne
        //          retourne un ModelView 

        // [public] appelle le service Habillage pour récupérer la liste des habillages d'un conseil

        // [public] appelle le service Photo pour récupérer la liste des photos d'un habillage


        // [public] appelle le service Photo pour récupérer la liste des photo selon le type de vêtement et le style en cours


        // [public] Cloturer un conseil

        // [public] Valider un habillage

        // [public] Soumettre une validation de conseil, avec la note



    }
}
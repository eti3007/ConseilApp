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
    public class RechercheBuilder : IRechercheBuilder
    {
        private IPersonneService _PersonneService;
        private int _StyleId ;
        private int _PersonneId;

        public RechercheBuilder(IPersonneService PersonneService)
        {
            this._PersonneService = PersonneService;
        }

        public void SetPersonneStyleIdentifier(int PersonneId, int StyleId)
        {
            this._StyleId = StyleId;
            this._PersonneId = PersonneId;
        }

        /// <summary>
        /// Page demande
        /// </summary>
        public IEnumerable<ApteConseil> DemandeAbonnePeutAider()
        {
            List<ApteConseil> result = new List<ApteConseil>();
            List<PersonViewItem> serviceObj = _PersonneService.ConseillersAptePourAider(_StyleId, _PersonneId);

            if (serviceObj != null)
            {
                ApteConseil apteConseil;
                foreach (PersonViewItem elem in serviceObj)
                {
                    apteConseil = new ApteConseil();

                    apteConseil.id = elem.Id;
                    apteConseil.pseudo = elem.Login;
                    apteConseil.nbDemande = elem.NumberA;
                    apteConseil.nbProposition = elem.NumberB;
                    apteConseil.note = elem.Note;
                    apteConseil.nbPhotoHabille = elem.NumberC;
                    if (!string.IsNullOrEmpty(elem.Genre)) apteConseil.genre = Convert.ToChar(elem.Genre);

                    result.Add(apteConseil);
                }
            }

            return result;
        }

        /// <summary>
        /// Page demande
        /// </summary>
        public IEnumerable<ProposeAide> DemandeAbonneProposeAider()
        {
            List<ProposeAide> result = new List<ProposeAide>();
            List<PersonViewItem> serviceObj = _PersonneService.ConseillersProposeAide(_StyleId, _PersonneId);

            if (serviceObj != null)
            {
                ProposeAide proposeAide;
                foreach (PersonViewItem elem in serviceObj)
                {
                    proposeAide = new ProposeAide();

                    proposeAide.id = elem.Id;
                    proposeAide.pseudo = elem.Login;
                    proposeAide.nbPhotoHabille = elem.NumberA;
                    proposeAide.nbAide = elem.NumberB;
                    proposeAide.nbHabPropose = elem.NumberC;
                    proposeAide.note = elem.Note;
                    if (!string.IsNullOrEmpty(elem.Genre)) proposeAide.genre = Convert.ToChar(elem.Genre);
                    if (elem.ConseilId.HasValue) proposeAide.conseilId = elem.ConseilId.Value;

                    result.Add(proposeAide);
                }
            }

            return result;
        }

        /// <summary>
        /// Page proposition
        /// </summary>
        public IEnumerable<AttenteConseil> PropositionAbonneAttenteConseil()
        {
            List<AttenteConseil> result = new List<AttenteConseil>();
            List<PersonViewItem> serviceObj = _PersonneService.AbonnesAttenteAide(_StyleId, _PersonneId);

            if (serviceObj != null)
            {
                AttenteConseil attenteConseil;
                foreach (PersonViewItem elem in serviceObj)
                {
                    attenteConseil = new AttenteConseil();

                    attenteConseil.id = elem.Id;
                    attenteConseil.pseudo = elem.Login;
                    attenteConseil.nbPhotoVetement = elem.NumberA;
                    attenteConseil.nbPhotoHabille = elem.NumberB;
                    if (!string.IsNullOrEmpty(elem.Genre)) attenteConseil.genre = Convert.ToChar(elem.Genre);

                    result.Add(attenteConseil);
                }
            }

            return result;
        }

        /// <summary>
        /// Page proposition
        /// </summary>
        public IEnumerable<SolliciteAide> PropositionAbonneSolliciteAide()
        {
            List<SolliciteAide> result = new List<SolliciteAide>();
            List<PersonViewItem> serviceObj = _PersonneService.AbonnesDemandeAide(_StyleId, _PersonneId);

            if (serviceObj != null)
            {
                SolliciteAide solliciteAide;
                foreach (PersonViewItem elem in serviceObj)
                {
                    solliciteAide = new SolliciteAide();

                    solliciteAide.id = elem.Id;
                    solliciteAide.pseudo = elem.Login;
                    solliciteAide.nbPhotoVetement = elem.NumberA;
                    solliciteAide.nbPhotoHabille = elem.NumberB;
                    if (!string.IsNullOrEmpty(elem.Genre)) solliciteAide.genre = Convert.ToChar(elem.Genre);
                    if (elem.ConseilId.HasValue) solliciteAide.conseilId = elem.ConseilId.Value;

                    result.Add(solliciteAide);
                }
            }

            return result;
        }
    }
}
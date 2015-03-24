using System.Collections.Generic;
using ConseilREP.Interfaces;
using ConseilBLL.Interfaces;
using ConseilOBJ;

namespace ConseilBLL
{
    public class ConseilService : IConseilService
    {
        IConseilRepository _ConseilRepository;

        public ConseilService(IConseilRepository ConseilRepository)
        {
            this._ConseilRepository = ConseilRepository;
        }

        /// <summary>
        /// Prépare les informations pour la création d'une aide
        /// </summary>
        /// <param name="conseilId">Identifiant du conseil ou nul</param>
        /// <param name="demandeurId">Identifiant du demandeur ou nul</param>
        /// <param name="conseillerId">Identifiant du conseiller ou nul</param>
        /// <param name="styleId">Identifiant du style concerné</param>
        /// <param name="pageDemande">Est-ce une demande ou une proposition qui est concernée</param>
        public void AppliqueActionAbonne(int? conseilId, int? demandeurId, int? conseillerId, int styleId, bool pageDemande)
        {
            NotifType typeNotification;

            // si il y a un conseilId
            if (conseilId.HasValue)
            {
                // on récupère/vérifie les informations du demandeur, conseiller, style
                var conseilObj = _ConseilRepository.GetById(conseilId.Value);
                if (conseilObj.ConseillerId != conseillerId.Value ||
                    conseilObj.DemandeurId != demandeurId.Value ||
                    conseilObj.StyleId != styleId) return;

                // détermine le type de notification selon la page
                typeNotification = NotifType.PropositionAccept;
                if (pageDemande) typeNotification = NotifType.DemandAccept;

                // effectue une modification
                _ConseilRepository.UpdDressingDemand(conseilId.Value, styleId, conseillerId.Value, demandeurId.Value, (int)DemandeStatus.Accepte, (int)typeNotification);

            }
            else
            {
                DemandeStatus stautDemande;
                // sinon on vérifie que demandeurId et conseillerId ne soit pas nul et qu'il n'y a pas déjà un conseil en cours avec le style en paramètre
                if (pageDemande) {
                    if (_ConseilRepository.ExistConseilByIds(styleId, conseillerId.Value, demandeurId.Value, (int)DemandeStatus.AttenteConseiller)) return;

                    // détermine le type de notification et le statut du conseil selon la page
                    typeNotification = NotifType.DemandCreation;
                    stautDemande = DemandeStatus.AttenteConseiller;
                }
                else{
                    if (_ConseilRepository.ExistConseilByIds(styleId, conseillerId.Value, demandeurId.Value, (int)DemandeStatus.AttenteDemandeur)) return;

                    // détermine le type de notification et le statut du conseil selon la page
                    typeNotification = NotifType.PropositionCreation;
                    stautDemande = DemandeStatus.AttenteDemandeur;
                }
                
                // effectue une création
                _ConseilRepository.AddDressingDemand(styleId, conseillerId.Value, demandeurId.Value, (int)stautDemande, (int)typeNotification);
            }
        }

        /// <summary>
        /// Récupère la liste des conseils pour un demandeur
        /// </summary>
        public List<Conseil> RecupereConseilsDemandeurParStatutStyle(List<int> statuses, int style, int personneId)
        {
            return this._ConseilRepository.GetByStatusesStylePerson(statuses, style, personneId);
        }

        /// <summary>
        /// Récupère la liste des conseils pour un conseiller
        /// </summary>
        public List<Conseil> RecupereConseilsConseillerParStatutStyle(List<int> statuses, int style, int personneId)
        {
            return this._ConseilRepository.GetByStatusesStylePerson(statuses, style, personneId, false);
        }

        /// <summary>
        /// Permet de terminer un conseil après que le conseiller ait créé au moins un habillage !
        /// </summary>
        public void TerminerConseil(int conseilId)
        {
            this._ConseilRepository.EndConseil(conseilId);
        }
    }
}

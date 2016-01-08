using System.Collections.Generic;
using ConseilREP.Interfaces;
using ConseilBLL.Interfaces;
using ConseilOBJ;

namespace ConseilBLL
{
    public class HabillageService : IHabillageService
    {
        IHabillageRepository _HabillageRepository;

        public HabillageService(IHabillageRepository HabillageRepository)
        {
            this._HabillageRepository = HabillageRepository;
        }

        /// <summary>
        /// Récupère la liste des habillages d'un conseil
        /// </summary>
        public List<Habillage> RecupereHabillagePourConseil(int conseilId)
        {
            return this._HabillageRepository.GetByConseilId(conseilId);
        }

        /// <summary>
        /// Sauvegarde (ajout ou modification) un habillage
        /// </summary>
        /// <remarks> à voir si on ajoute la gestion des images dans cette méthode si c'est une création d'habillage !!!! </remarks>
        public int SauvegardeHabillage(int? habillageId, int conseilId, System.DateTime jour, short? note)
        {
            return this._HabillageRepository.SaveHabillage(habillageId, conseilId, jour, note);
        }

        public int SauvegardePhotosHabillage(int habillageId, List<int> picsId)
        {
            return this._HabillageRepository.AddPicsToHabillage(habillageId, picsId);
        }

        public bool SupprimeHabillage(int habillageId)
        {
            return this._HabillageRepository.DeleteHabillage(habillageId);
        }
    }
}

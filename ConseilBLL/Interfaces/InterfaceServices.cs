using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConseilOBJ;

namespace ConseilBLL.Interfaces
{
    public interface IPersonneService
    {
        int AjouteNouveau(Personne personne, int styleId, string pseudo);
        Personne RecuperePersonneConnecte(string pseudo);

        List<PersonViewItem> ConseillersAptePourAider(int styleId, int personneId);
        List<PersonViewItem> ConseillersProposeAide(int styleId, int personneId);
        List<PersonViewItem> AbonnesAttenteAide(int styleId, int personneId);
        List<PersonViewItem> AbonnesDemandeAide(int styleId, int personneId);
    }

    public interface IStyleService
    {
        List<Style> RecupereListeDesStyles();
        List<Style> RecupereListeDesStylesConseiller(int personneId);
        List<DropDownListeStyle> RecupereListeDesStylesPourDDL();
        List<DropDownListeStyle> RecupereListeDesStylesConseillerPourDDL(int personneId);
    }

    public interface ISuiviTelechargementService
    {
        void ConfigureSuivieTelechargement(DateTime jour, int personneId, bool estNouveau);

        int NbPhotoTelechargeable(DateTime jour, int personneId);
    }

    public interface ITypeVetementService
    {
        List<TypeParam> RecupereListeDesTypesVetement();
        List<DropDownListeTypeParam> RecupereListeDesTypesVetementPourDDL();
    }

    public interface IStatutHistoriqueService
    {
        int RecupereStatusPourPersonneEtStyle(int personneId, int styleId);
        void MajStyleStatutByPersonne(int personneId, int styleId, bool enAttente);
    }

    public interface IPhotoService
    {
        void AjoutePhotoVetement(int styleId, bool enAttente, int personneId, int vetementId, List<string> urlList);
        void AjoutePhotoHabillage(int styleId, int personneId, List<string> urlList);
        List<Photo> RecuperePhotosPourPersonneStyleVetement(int personneId, int styleId, int vetementId);
        List<Photo> RecuperePhotosPourPersonneStyle(int personneId, PhotoType typePhoto, int styleId);
        bool SupprimePhotosParPersonneStyleVetement(int personneId, int styleId, int vetementId, string photoNom);
        bool SupprimePhotosHabillageParPersonneStyle(int personneId, int styleId, string photoNom);
        List<Photo> RecuperePhotosPourHabillage(int habillageId);
    }

    public interface IVetementService
    {
        List<Vetement> RecupereListeDesVetements();
        List<DropDownListeVetement> RecupereListeDesVetementsPourDDL();
    }

    public interface IConseilService
    {
        void AppliqueActionAbonne(int? conseilId, int? demandeurId, int? conseillerId, int styleId, bool pageDemande);
        Dictionary<int, string[]> RecupereConseilsDemandeurParStatutStyle(List<int> statuses, int style, int personneId);
        Dictionary<int, string[]> RecupereConseilsConseillerParStatutStyle(List<int> statuses, int style, int personneId);
        void TerminerConseil(int conseilId);
    }

    public interface INotificationService
    {
        List<Notification> RecupereListeNotification(int styleId, int personneId, bool isDemandeur);
        bool PersonneEnvoiMessage(int conseilId, int personneId, string message);
    }

    public interface IHabillageService
    {
        List<Habillage> RecupereHabillagePourConseil(int conseilId);
        int SauvegardeHabillage(int? habillageId, int conseilId, System.DateTime jour, short? note);
        int SauvegardePhotosHabillage(int habillageId, List<int> picsId);
    }
}

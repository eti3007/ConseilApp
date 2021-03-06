﻿using System;
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
        List<DropDownListeStyle> RecupereListeDesStylesPourDDL();
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
    }

    public interface IPhotoService
    {
        void AjoutePhotoVetement(int styleId, bool enAttente, int personneId, int vetementId, List<string> urlList);
        void AjoutePhotoHabillage(int styleId, int personneId, List<string> urlList);
        List<Photo> RecuperePhotosPourPersonneStyleVetement(int personneId, int styleId, int vetementId);
        List<Photo> RecuperePhotosPourPersonneStyle(int personneId, PhotoType typePhoto, int styleId);
        bool SupprimePhotosParPersonneStyleVetement(int personneId, int styleId, int vetementId, string photoNom);
        bool SupprimePhotosHabillageParPersonneStyle(int personneId, int styleId, string photoNom);
    }

    public interface IVetementService
    {
        List<Vetement> RecupereListeDesVetements();
        List<DropDownListeVetement> RecupereListeDesVetementsPourDDL();
    }
}

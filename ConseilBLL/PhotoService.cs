﻿using System.Collections.Generic;
using ConseilREP.Interfaces;
using ConseilBLL.Interfaces;
using ConseilOBJ;

namespace ConseilBLL
{
    public class PhotoService : IPhotoService
    {
        IPhotoRepository repository;

        public PhotoService(IPhotoRepository repo)
        {
            repository = repo;
        }

        public void AjoutePhotoVetement(int styleId, bool enAttente, int personneId, int vetementId, List<string> urlList)
        {
            try
            {
                repository.AddClothesPics(styleId, enAttente, personneId, vetementId, urlList);
            }
            catch
            {
                throw;
            }
        }

        public void AjoutePhotoHabillage(int styleId, int personneId, List<string> urlList)
        {
            try
            {
                repository.AddWearingPics(styleId, personneId, urlList);
            }
            catch
            {
                throw;
            }
        }

        public List<Photo> RecuperePhotosPourPersonneStyleVetement(int personneId, int styleId, int vetementId)
        {
            return repository.GetPicsByPersonVetementStyle(personneId, styleId, vetementId);
        }

        public List<Photo> RecuperePhotosPourPersonneStyleTypeVetement(int personneId, int styleId, int typeVetementId)
        {
            return repository.GetPicsByPersonTypeVetementStyle(personneId, styleId, typeVetementId);
        }

        public List<Photo> RecuperePhotosPourPersonneStyle(int personneId, PhotoType typePhoto, int styleId)
        {
            return repository.GetPicsByPersonStyle(personneId, typePhoto, styleId);
        }

        public bool EstHabillageValide(List<int> photos)
        {
            return repository.IsHabillageValide(photos);
        }

        public bool SupprimePhotosParPersonneStyleVetement(int personneId, int styleId, int vetementId, string photoNom)
        {
            return repository.DeletePicByPersonStyleVetement(personneId, styleId, vetementId, photoNom);
        }

        public bool SupprimePhotosHabillageParPersonneStyle(int personneId, int styleId, string photoNom)
        {
            return repository.DeleteWearingPicByPersonStyle(personneId, styleId, photoNom);
        }

        public List<Photo> RecuperePhotosPourHabillage(int habillageId)
        {
            return this.repository.GetPicsByHabillage(habillageId);
        }
    }
}

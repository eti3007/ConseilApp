using System.Collections.Generic;
using ConseilOBJ;

namespace ConseilREP.Interfaces
{
    public interface IPhotoRepository
    {
        void AddClothesPics(int styleId, bool enAttente, int personneId, int vetementId, List<string> urlList);

        void AddWearingPics(int styleId, int personneId, List<string> urlList);

        List<Photo> GetPicsByTypeAndPerson(int personneId, PhotoType typeId, int vetementId = 0);

        List<Photo> GetPicsByPersonStyle(int personneId, PhotoType typeId, int styleId);

        List<Photo> GetPicsByPersonVetementStyle(int personneId, int styleId, int vetementId);

        List<Photo> GetPicsByPersonTypeVetementStyle(int personneId, int styleId, int TypeParamId);

        List<Photo> GetPicsByHabillage(int habillageId);

        bool IsHabillageValide(List<int> photos);

        bool DeletePicByPersonStyleVetement(int personneId, int styleId, int vetementId, string photoNom);

        bool DeleteWearingPicByPersonStyle(int personneId, int styleId, string photoNom);
    }
}

using System.Collections.Generic;
using ConseilOBJ;

namespace ConseilApp.Builders.Interfaces
{
    public interface IPhotoBuilder
    {
        List<string> UrlPhotoListe(List<Photo> photos);
    }
}

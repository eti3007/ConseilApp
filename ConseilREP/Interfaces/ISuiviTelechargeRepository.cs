using System;
using System.Collections.Generic;

namespace ConseilREP.Interfaces
{
    public interface ISuiviTelechargeRepository
    {
        void Add(DateTime jour, int personneId, bool estNouveau);

        int NbPhotoToUpload(DateTime jour, int personneId);
    }
}

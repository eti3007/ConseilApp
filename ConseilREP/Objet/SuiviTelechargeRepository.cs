using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Validation;
using System.Data.Entity;
using ConseilOBJ;
using ConseilDAL;
using ConseilDAL.Exceptions;
using ConseilREP.Interfaces;

namespace ConseilREP
{
    public class SuiviTelechargeRepository : ISuiviTelechargeRepository, IDisposable
    {
        /// <summary>
        /// Configure le nombre de photo pour une personne selon le jour en cours
        /// </summary>
        /// <param name="jour">jour en cours</param>
        /// <param name="personneId">personne</param>
        /// <param name="estNouveau">nouvel abonné ?</param>
        public void Add(DateTime jour, int personneId, bool estNouveau)
        {
            using (var context = new ConseilEntitiesBis())
            {
                try
                {
                    // **** Récupère le nombre de photo max par jour pour un abonné ****
                    Byte compteurTelecharge = 0;
                    TypeParam compteurAbonne;
                    if (estNouveau)
                        compteurAbonne = context.TypeParams.Where(c => c.TypeId == 6 && c.ParamCode == "TCAbonne").FirstOrDefault();                     
                    else
                        compteurAbonne = context.TypeParams.Where(c => c.TypeId == 6 && c.ParamCode == "TCConseiller").FirstOrDefault();                    
                    if (compteurAbonne != null) compteurTelecharge = Convert.ToByte(compteurAbonne.ParamLib);

                    // **** SuiviTelecharge ****
                    SuiviTelecharge suivieUpload = new SuiviTelecharge();
                    suivieUpload.Jour = jour;
                    suivieUpload.Compteur = compteurTelecharge;
                    suivieUpload.PersonneId = personneId;

                    context.Entry(suivieUpload).State = EntityState.Added;
                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    throw new CustomException().CustomValidationExceptionReturn(ex);
                }
            }
        }


        // méthode qui est appelée quand l'utilisateur affiche la page de téléchargement des photos
        // cette méthode vérifie qu'il a un suivi pour le jour en cours (à voir si on fait un suivie hebdomadaire)


        /// <summary>
        /// Méthode qui retourne le nombre de photo que la personne peut télécharger
        /// </summary>
        /// <param name="jour">jour en cours</param>
        /// <param name="personneId">personne</param>
        /// <returns>nombre de photo téléchargeable</returns>
        public int NbPhotoToUpload(DateTime jour, int personneId)
        {
            int nb = 0;
            int defaultNb = 0;
            SuiviTelecharge suivie = null;

            using (var context = new ConseilEntitiesBis())
            {
                suivie = context.SuiviTelecharges.Where(c => c.PersonneId == personneId).OrderByDescending(c => c.Jour).FirstOrDefault();

                var compteurAbonne = context.TypeParams.Where(c => c.TypeId == 6 && c.ParamCode == "TCAbonne").FirstOrDefault();
                if (compteurAbonne != null) defaultNb = Convert.ToByte(compteurAbonne.ParamLib);
            }

            if (suivie != null)
            {
                if(suivie.Jour.ToShortDateString() == jour.ToShortDateString())
                    nb = Convert.ToInt32(suivie.Compteur);
            }
            else
            {
                this.Add(jour, personneId, true);
                nb = defaultNb;
            }

           return nb;
        }

        public void Dispose()
        {
        }
    }
}

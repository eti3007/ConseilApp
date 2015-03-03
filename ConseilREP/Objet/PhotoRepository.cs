using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Data.Entity.Validation;
using System.Data.Entity;
using ConseilOBJ;
using ConseilDAL;
using ConseilDAL.Exceptions;
using ConseilREP.Interfaces;

namespace ConseilREP
{
    public class PhotoRepository : IPhotoRepository, IDisposable
    {
        /// <summary>
        /// Ajoute une liste de photo de vêtement et met à jours le statut de la personne
        /// </summary>
        /// <param name="styleId">style concerné</param>
        /// <param name="enAttente">en attente d'aide ?</param>
        /// <param name="personneId">personne</param>
        /// <param name="vetementId">vêtement concerné</param>
        /// <param name="urlList">liste d'url des photos</param>
        public void AddClothesPics(int styleId, bool enAttente, int personneId, int vetementId, List<string> urlList)
        {
            using (var context = new ConseilEntitiesBis())
            {
                // Vérification sur les images existantes en base
                var nbPhoto = context.Photos.Count(c => (c.PersonneId == personneId));
                if (nbPhoto > 0) {
                    var lst = context.Photos.Include(c => c.Styles)
                                                .Where(c => (c.PersonneId == personneId) &&
                                                            (c.VetementId == vetementId) &&
                                                            (c.Styles.Count(v => v.Id == styleId) > 0))
                                                .Select(c => c.Url).ToList();
                    foreach (string s in lst) { urlList.Remove(s); }
                    lst = null;
                }

                using (TransactionScope transaction = new TransactionScope())
                {
                    try
                    {
                        // Vérifie l'existance du Style en cours
                        Style styleContext = context.Styles.Where(c => c.Id == styleId).FirstOrDefault();
                        if (styleContext == null) return;

                        // Vérifie l'existance de la personne
                        Personne personneContext = context.Personnes.Where(c => c.Id == personneId).FirstOrDefault();
                        if (personneContext == null) return;

                        // Vérifie l'existance du vêtement et récupère le type de vêtement
                        /*  TO DO  */

                        StatutHistorique statutToAdd = null;

                        // vérifie que l'utilisateur est déjà associé à ce style dans la table StatutHsitorique
                        statutToAdd = context.StatutHistoriques.Where(c => c.PersonneId == personneId && c.StyleId == styleId).FirstOrDefault();
                        if (statutToAdd == null)
                        {
                            // si l'utilisateur n'est pas associé à ce style, 
                            // on ajoute le style à l'historique de la personne
                            statutToAdd = new StatutHistorique();
                            statutToAdd.PersonneId = personneId;
                            statutToAdd.StyleId = styleId;
                            statutToAdd.DateCreation = DateTime.Now;
                            statutToAdd.TypeId = enAttente ? (int)PersonneStatus.EnAttente : (int)PersonneStatus.Abonne;

                            context.StatutHistoriques.Attach(statutToAdd);
                            context.Entry(statutToAdd).State = EntityState.Added;
                        }
                        else
                        {
                            // vérifie le type de statut pour le style
                            //if (enAttente && statutToAdd.TypeId == (int)PersonneStatus.Abonne) statutToAdd.TypeId = (int)PersonneStatus.EnAttente;
                            statutToAdd.TypeId = enAttente ? (int)PersonneStatus.EnAttente : (int)PersonneStatus.Abonne;

                            context.StatutHistoriques.Attach(statutToAdd);
                            context.Entry(statutToAdd).State = EntityState.Modified;
                        }
                        context.SaveChanges();

                        this.AddPictureToDB(context, urlList, personneId, styleContext, vetementId, PhotoType.Vetement);

                         // Mark the transaction as complete.
                        transaction.Complete();
                    }
                    catch (DbEntityValidationException ex)
                    {
                        throw new CustomException().CustomValidationExceptionReturn(ex);
                    }
                }
            }
        }

        /// <summary>
        /// Ajoute une liste de photo d'habillage de la personne
        /// </summary>
        /// <param name="styleId">style concerné</param>
        /// <param name="personneId">personne</param>
        /// <param name="urlList">liste d'url des photos</param>
        public void AddWearingPics(int styleId, int personneId, List<string> urlList)
        {
            using (var context = new ConseilEntitiesBis())
            {
                // Vérification sur les images existantes en base
                var lst = context.Photos.Include(c => c.Styles)
                                            .Where(c => (c.PersonneId == personneId) &&
                                                        (c.Styles.Count(v => v.Id == styleId) > 0))
                                            .Select(c => c.Url).ToList();
                foreach (string s in lst) { urlList.Remove(s); }
                lst = null;
                using (TransactionScope transaction = new TransactionScope())
                {
                    try
                    {
                        // Vérifie l'existance du Style en cours
                        Style styleContext = context.Styles.Where(c => c.Id == styleId).FirstOrDefault();
                        if (styleContext == null) return;

                        // Vérifie l'existance de la personne
                        Personne personneContext = context.Personnes.Where(c => c.Id == personneId).FirstOrDefault();
                        if (personneContext == null) return;

                        // Vérifie que son statut est toujours celui de conseiller
                        var lastStatus = context.StatutHistoriques.AsQueryable().Where(c => c.PersonneId == personneId && c.StyleId == styleId).ToList().OrderByDescending(x => x.DateCreation).Select(b => new {b.TypeId}).FirstOrDefault();
                        if (lastStatus == null || lastStatus.TypeId != (int)PersonneStatus.Conseiller) return;

                        this.AddPictureToDB(context, urlList, personneId, styleContext, null, PhotoType.Habille);

                        // Mark the transaction as complete.
                        transaction.Complete();
                    }
                    catch (DbEntityValidationException ex)
                    {
                        throw new CustomException().CustomValidationExceptionReturn(ex);
                    }
                }
            }
        }

        private void AddPictureToDB(ConseilEntitiesBis context, List<string> urlList, int personneId, Style styleContext, int? vetementId, PhotoType type)
        {
            // ajoute les urls dans la table Photo
            Photo photoToAdd = null;
            DateTime dtTelecharge = DateTime.Now;
            foreach (string url in urlList)
            {
                photoToAdd = new Photo();
                photoToAdd.PersonneId = personneId;
                photoToAdd.TypeId = (int)type;
                photoToAdd.Url = url;
                photoToAdd.VetementId = vetementId;
                photoToAdd.DateTelecharge = dtTelecharge;
                photoToAdd.Styles.Add(styleContext);

                context.Photos.Attach(photoToAdd);
                context.Entry(photoToAdd).State = EntityState.Added;
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Récupère les photos d'une personne selon le type
        /// </summary>
        /// <param name="personneId">personne</param>
        /// <param name="typeId">type de photo</param>
        /// <param name="vetementId">vêtement concerné</param>
        /// <returns>liste de photo</returns>
        public List<Photo> GetPicsByTypeAndPerson(int personneId, PhotoType typeId, int vetementId = 0)
        {
            List<Photo> result = null;

            using (var context = new ConseilEntitiesBis())
            {
                try
                {
                    if (typeId == PhotoType.Vetement)
                        result = context.Photos.Where(c => (c.PersonneId == personneId) &&
                                                          (c.VetementId == (vetementId == 0 ? c.VetementId : vetementId)) && 
                                                          (c.TypeId == (int)typeId)).ToList();
                    else
                        result = context.Photos.Where(c => (c.PersonneId == personneId) && (c.TypeId == (int)typeId)).ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    throw new CustomException().CustomValidationExceptionReturn(ex);
                }
            }
            return result;
        }

        /// <summary>
        /// Récupère les photos pour une personne selon un type de photo et un style
        /// </summary>
        public List<Photo> GetPicsByPersonStyle(int personneId, PhotoType typeId, int styleId)
        {
            List<Photo> result = null;

            using (var context = new ConseilEntitiesBis())
            {
                try
                {
                    var nbPhoto = context.Photos.Count(c => (c.PersonneId == personneId));
                    if (nbPhoto <= 0) return null;

                    result = context.Photos.Include(c => c.Styles)
                                           .Where(c => (c.PersonneId == personneId) && 
                                                       (c.TypeId == (int)typeId) &&
                                                       (c.Styles.Count(v => v.Id == styleId) > 0))
                                           .ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    throw new CustomException().CustomValidationExceptionReturn(ex);
                }
            }
            return result;
        }

        /// <summary>
        /// Récupère les photos de vêtement pour une personne selon un style et type de vètement
        /// </summary>
        public List<Photo> GetPicsByPersonVetementStyle(int personneId, int styleId, int vetementId)
        {
            List<Photo> result = null;

            using (var context = new ConseilEntitiesBis())
            {
                try
                {
                    var nbPhoto = context.Photos.Count(c => (c.PersonneId == personneId));
                    if (nbPhoto <= 0) return null;

                    result = context.Photos.Include(c => c.Styles)
                                           .Where(c => (c.PersonneId == personneId) &&
                                                       (c.TypeId == (int)PhotoType.Vetement) &&
                                                       (c.VetementId == vetementId) && 
                                                       (c.Styles.Count(v => v.Id == styleId) > 0))
                                           .ToList();
                }
                catch (DbEntityValidationException ex)
                {
                    throw new CustomException().CustomValidationExceptionReturn(ex);
                }
            }
            return result;
        }

        /// <summary>
        /// Permet de supprimer la photo d'un vêtement d'une personne pour un style
        /// </summary>
        public bool DeletePicByPersonStyleVetement(int personneId, int styleId, int vetementId, string photoNom)
        {
            bool result = false;
            using (var context = new ConseilEntitiesBis())
            {
                /* récupère l'objet photo correspondant :
                 *      - nom de l'image
                 *      - type de la photo
                 *      - le vêtement
                 *      - la personne
                 */
                var photo = context.Photos.Include("Styles")
                                          .Where(p => p.PersonneId == personneId &&
                                                      p.TypeId == (int)PhotoType.Vetement &&
                                                      p.VetementId == vetementId &&
                                                      p.Url == photoNom &&
                                                      p.Styles.Count(s => s.Id == styleId) == 1)
                                          .FirstOrDefault();

                var photos = context.Photos.Include("Styles")
                                          .Where(p => p.PersonneId == personneId &&
                                                      p.TypeId == (int)PhotoType.Vetement &&
                                                      p.Url == photoNom &&
                                                      p.Styles.Count(s => s.Id == styleId) == 1)
                                          .ToList();

                // est-ce que cette photo existe uniquement pour le style en paramètre ?
                
                var style = photo.Styles.First(s => s.Id == styleId);
                if (photo != null)
                {
                    // OUI : on supprime dans les tables : Photo et PhotoStyle
                    context.Photos.Remove(photo);
                    context.Entry(photo).State = EntityState.Deleted;
                    context.SaveChanges();
                    result = true;
                }

                if (photos != null) result = photos.Count == 1 ? true : false;
            }
            return result;
        }
        public bool DeleteWearingPicByPersonStyle(int personneId, int styleId, string photoNom)
        {
            bool result = false;
            using (var context = new ConseilEntitiesBis())
            {
                /* récupère l'objet photo correspondant :
                 *      - nom de l'image
                 *      - type de la photo
                 *      - la personne
                 */
                var photo = context.Photos.Include("Styles")
                                          .Where(p => p.PersonneId == personneId &&
                                                      p.TypeId == (int)PhotoType.Habille &&
                                                      p.Url == photoNom &&
                                                      p.Styles.Count(s => s.Id == styleId) == 1)
                                          .FirstOrDefault();

                //var photos = context.Photos.Include("Styles")
                //                          .Where(p => p.PersonneId == personneId &&
                //                                      p.TypeId == (int)PhotoType.Habille &&
                //                                      p.Url == photoNom &&
                //                                      p.Styles.Count(s => s.Id == styleId) == 1)
                //                          .ToList();

                // est-ce que cette photo existe uniquement pour le style en paramètre ?

                var style = photo.Styles.First(s => s.Id == styleId);
                if (photo != null)
                {
                    // OUI : on supprime dans les tables : Photo et PhotoStyle
                    context.Photos.Remove(photo);
                    context.Entry(photo).State = EntityState.Deleted;
                    context.SaveChanges();
                    result = true;
                }

                //if (photos != null) result = photos.Count == 1 ? true : false;
            }
            return result;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {

        }
    }
}

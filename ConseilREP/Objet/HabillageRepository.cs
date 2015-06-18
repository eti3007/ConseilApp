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
    public class HabillageRepository : IHabillageRepository, IDisposable
    {
        /// <summary>
        /// Retourne la liste des habillages d'un conseil
        /// </summary>
        /// <param name="id">identifiant du conseil</param>
        /// <returns>liste d'habillage</returns>
        public List<Habillage> GetByConseilId(int id)
        {
            using (var context = new ConseilEntitiesBis())
            {
                return context.Habillages.AsQueryable().Where(h => h.ConseilId.Equals(id)).ToList();
            }
        }

        /// <summary>
        /// Créé ou modifie un habillage
        /// </summary>
        /// <param name="habillageId">identifiant de l'habillage pour la modification (peut être nul)</param>
        /// <param name="conseilId">conseil concerné</param>
        /// <param name="jour">jour de l'ajout/modif</param>
        /// <param name="note">note attribuée par le demandeur</param>
        /// <returns>identifiant de l'habillage</returns>
        public int SaveHabillage(int? habillageId, int conseilId, DateTime jour, short? note)
        {
            int result = 0;
            using (var context = new ConseilEntitiesBis())
            {
                try
                {
                    Habillage habillage = null;

                    // Si on a un id d'habillage alors on effectue une modification
                    if (habillageId.HasValue)
                    {
                        habillage = context.Habillages.AsQueryable().Where(x => x.Id.Equals(habillageId.Value)).FirstOrDefault();
                        if (habillage != null)
                        {
                            habillage.Note = note;
                            context.Entry(habillage).State = EntityState.Modified;
                        }
                    }
                    // Sinon alors on effectue un ajout
                    else
                    {
                        habillage = new Habillage()
                        {
                            ConseilId = conseilId,
                            DateCreation = jour
                        };
                        context.Entry(habillage).State = EntityState.Added;
                    }

                    if (habillage != null)
                    {
                        context.SaveChanges();
                        result = habillage.Id;
                    }

                    habillage = null;
                }
                catch (DbEntityValidationException ex)
                {
                    throw new CustomException().CustomValidationExceptionReturn(ex);
                }
                catch
                {
                    return -1;
                }
            }
            return result;
        }

        /// <summary>
        /// Associe l'habillage à une liste de photo
        /// </summary>
        /// <param name="habillageId">identifiant de l'habillage</param>
        /// <param name="picsId">liste d'identifiant de photo</param>
        /// <returns>nombre de photo associé à l'habillage</returns>
        public int AddPicsToHabillage(int habillageId, List<int> picsId)
        {
            int result = 0;

            using (var context = new ConseilEntitiesBis())
            {
                try
                {
                    Habillage habillage = context.Habillages.Include("Photos").AsQueryable().FirstOrDefault(h => h.Id.Equals(habillageId));
                    if (habillage != null)
                    {
                        if (habillage.Photos == null) { habillage.Photos = new List<Photo>(); }
                        foreach (int i in picsId)
                        {
                            if (context.Photos.AsQueryable().Any(p => p.Id.Equals(i)))
                                habillage.Photos.Add(context.Photos.AsQueryable().First(p => p.Id.Equals(i)));
                        }

                        context.Entry(habillage).State = EntityState.Modified;
                        context.SaveChanges();

                        habillage = null;
                        result = context.Habillages.Include("Photos").AsQueryable().Where(h => h.Id.Equals(habillageId)).First().Photos.Count();
                    }
                }
                catch (DbEntityValidationException ex)
                {
                    throw new CustomException().CustomValidationExceptionReturn(ex);
                }
                catch
                {
                    return -1;
                }
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

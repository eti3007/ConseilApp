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
    public class PersonneRepository : IPersonneRepository, IDisposable
    {
        public int AddNew(Personne personneToAdd, int styleId, string pseudo)
        {
            int result = 0;
            using (var context = new ConseilEntitiesBis())
            {
                try
                {
                    int userProfilId = context.UserProfiles.Where(c => c.UserName == pseudo).Select(c => c.UserId).FirstOrDefault();
                    if (userProfilId > 0)
                    {
                        personneToAdd.Id = userProfilId;
                        this.AddNew(personneToAdd, styleId);
                        result = userProfilId;
                    }
                }
                catch
                { return result; }
            }
            return result;
        }

        /// <summary>
        /// Ajoute un nouvel abonné ainsi que son style de référence
        /// </summary>
        /// <param name="personne">Information de la personne</param>
        /// <param name="styleId">Style de référence</param>
        /// <returns></returns>
        private bool AddNew(Personne personneToAdd, int styleId)
        {
            using (var context = new ConseilEntitiesBis())
            {
                try
                {
                    // **** StatutHistorique ****
                    StatutHistorique statutAbonne = new StatutHistorique();
                    statutAbonne.StyleId = styleId;
                    statutAbonne.DateCreation = DateTime.Now;
                    statutAbonne.TypeId = (int)PersonneStatus.Abonne;

                    // Ajoute l'enregistrement de l'historique du statut
                    personneToAdd.StatutHistoriques.Add(statutAbonne);

                    context.Entry(personneToAdd).State = EntityState.Added;
                    context.SaveChanges();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException e)
                {
                    var outputLines = new List<string>();
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        outputLines.Add(string.Format(
                            "{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:",
                            DateTime.Now, eve.Entry.Entity.GetType().Name, eve.Entry.State));
                        foreach (var ve in eve.ValidationErrors)
                        {
                            outputLines.Add(string.Format(
                                "- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage));
                        }
                    }
                    System.IO.File.AppendAllLines(@"c:\temp\errors.txt", outputLines);
                    throw;
                }
                catch
                {
                    return false;
                }
            }

            try
            {
                // Appel la methode d'ajout des informations de téléchargement
                SuiviTelechargeRepository objSuiviTelecharge = new SuiviTelechargeRepository();
                objSuiviTelecharge.Add(DateTime.Now, personneToAdd.Id, true);
            }
            catch
            {
                // rien à faire...
            }

            return true;
        }

        /// <summary>
        /// liste des conseillers pour un style disponible qui n'ont pas encore proposés leur aide
        /// </summary>
        /// <param name="styleId">style context</param>
        /// <param name="personneId">subscriber</param>
        /// <remarks>page "Mes demandes"</remarks>
        public List<PersonViewItem> GetListenersFreeToHelp(int styleId, int personneId)
        {
            List<PersonViewItem> result = null;

            using (var context = new ConseilEntitiesBis())
            {
                // Récupère la liste des conseillers en cours d'aide pour cet abonné sur ce style :
                var lstConseillerEnCours = context.Conseils.Where(c => c.StyleId == styleId &&
                                           (c.TypeId == (int)DemandeStatus.AttenteDemandeur || c.TypeId == (int)DemandeStatus.Accepte) &&
                                           c.DemandeurId == personneId).Select(c => c.ConseillerId).ToList();

                // récupère les conseillers qui n'ont pas encore proposer leur aide à la personne connectée
                var lstConseiller = (from p in context.Personnes
                                     join ph in context.Photos on p.Id equals ph.PersonneId
                                     join sh in context.StatutHistoriques on p.Id equals sh.PersonneId
                                     where p.Id != personneId &&
                                           sh.TypeId == (int)PersonneStatus.Conseiller && sh.StyleId == styleId &&
                                           !lstConseillerEnCours.Contains(p.Id)
                                     select new { Personne = p.Id, Pseudo = p.Pseudo, Genre = p.Genre }).Distinct().ToList();

                // récupère ceux 
                result = new List<PersonViewItem>();
                foreach (var v in lstConseiller)
                    result.Add(GetListenerInfos(context, new PersonViewItem() { Id = v.Personne, Login = v.Pseudo, Genre = v.Genre }, styleId));
            }

            return result;
        }

        /// <summary>
        /// liste des conseillers qui ont proposés leur aide
        /// </summary>
        /// <param name="styleId">style context</param>
        /// <param name="personneId">subscriber</param>
        /// <remarks>page "Mes demandes"</remarks>
        public List<PersonViewItem> GetListenersWantToHelp(int styleId, int personneId)
        {
            List<PersonViewItem> result = null;

            using (var context = new ConseilEntitiesBis())
            {
                // liste des conseiller qui ont proposer leur aide
                var lstConseiller = (from p in context.Personnes
                                            join c in context.Conseils on p.Id equals c.ConseillerId
                                            where c.TypeId == (int)DemandeStatus.AttenteDemandeur && c.ConseillerId != personneId &&
                                            c.DemandeurId == personneId && c.StyleId == styleId
                                     select new { Personne = p.Id, Pseudo = p.Pseudo, Genre = p.Genre, Conseil = c.Id }).Distinct().ToList();

                result = new List<PersonViewItem>();
                foreach (var v in lstConseiller)
                    result.Add(GetListenerInfos(context, new PersonViewItem() { Id = v.Personne, Login = v.Pseudo, Genre = v.Genre, ConseilId = v.Conseil }, styleId));
            }

            return result;
        }

        /// <summary>
        /// liste des abonnés en attente d'aide pour le style en cours
        /// pseudo, nb photo vêtement, nb photo habillé, genre
        /// </summary>
        /// <param name="styleId">style context</param>
        /// <param name="personneId">listener</param>
        /// <remarks>page "Mes propositions"</remarks>
        public List<PersonViewItem> GetSubscibersWaitForHelp(int styleId, int personneId)
        {
            List<PersonViewItem> result = null;
            using (var context = new ConseilEntitiesBis())
            {
                // récupère la liste des abonnés en attente d'aide pour un style :
                var lstAbonnes = (from p in context.Personnes.Include("StatutHistoriques")
                                  where p.StatutHistoriques.Where(o => o.StyleId == styleId && o.TypeId == (int)PersonneStatus.EnAttente).Count() > 0
                                  select new { Personne = p.Id, Pseudo = p.Pseudo, Genre = p.Genre }).Distinct().ToList();

                // récupère la liste des abonnés pour qui le conseiller a une aide en cours
                var lstAideEnCours = (from c in context.Conseils
                                      join p in context.Personnes on c.DemandeurId equals p.Id
                                      where c.StyleId==styleId && c.ConseillerId == personneId && 
                                      (c.TypeId == (int)DemandeStatus.AttenteDemandeur || c.TypeId == (int)DemandeStatus.Accepte)
                                      select new { Personne = p.Id, Pseudo = p.Pseudo, Genre = p.Genre }).Distinct().ToList();



                result = new List<PersonViewItem>();
                foreach (var a in lstAbonnes)
                    // si l'élément n'est pas présent dans la liste des aides en cours alors on l'ajoute dans le résultat
                    if (!lstAideEnCours.Contains(a))
                    result.Add(this.GetSubscriberInfos(context, new PersonViewItem() { Id = a.Personne, Login = a.Pseudo, Genre = a.Genre }, styleId));
            }
            return result;
        }

        /// <summary>
        /// liste des abonnés qui ont demander de l'aide pour le style en cours au conseiller connecté
        /// pseudo, nb photo vêtement, nb photo habillé, genre
        /// </summary>
        /// <param name="styleId">style context</param>
        /// <param name="personneId">listener</param>
        /// <remarks>page "Mes propositions"</remarks>
        public List<PersonViewItem> GetSubscribersAskForHelp(int styleId, int personneId)
        {
            List<PersonViewItem> result = null;
            using (var context = new ConseilEntitiesBis())
            {
                var lstAbonnes = (from p in context.Personnes.Include("StatutHistoriques")
                                  join c in context.Conseils on p.Id equals c.DemandeurId
                                  where c.ConseillerId == personneId 
                                  && c.StyleId == styleId 
                                  && c.TypeId == (int)DemandeStatus.AttenteConseiller 
                                  && p.StatutHistoriques.Where(o => o.StyleId == styleId && o.TypeId == (int)PersonneStatus.EnAttente).Count() > 0
                                  select new { Personne = p.Id, Pseudo = p.Pseudo, Genre = p.Genre, Conseil = c.Id }).Distinct().ToList();

                result = new List<PersonViewItem>();
                foreach (var a in lstAbonnes)
                    result.Add(this.GetSubscriberInfos(context, new PersonViewItem() { Id = a.Personne, Login = a.Pseudo, Genre = a.Genre, ConseilId = a.Conseil }, styleId));
            }
            return result;
        }

        /// <summary>
        /// Récupère les infos de la personne et son style par défaut
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Personne GetPersonneLogin(string login)
        {
            Personne result = null;

            using (var context = new ConseilEntitiesBis())
            {
                result = context.Personnes.AsQueryable().Where(c => c.Pseudo == login).FirstOrDefault();

                if (result != null && result.StatutHistoriques == null)
                {
                    result.StatutHistoriques = new List<StatutHistorique>();
                    result.StatutHistoriques = context.StatutHistoriques.AsQueryable().Where(c => c.PersonneId == result.Id).ToList();
                }
            }

            return result;
        }


        private PersonViewItem GetListenerInfos(ConseilEntitiesBis context, PersonViewItem personne, int style)
        {
            var obj = new PersonViewItem();
            obj.Id = personne.Id;
            obj.Login = personne.Login;
            obj.Genre = personne.Genre;

            // nombre de demande reçu
            obj.NumberA = this.CountReceiveDemands(context, personne.Id, style);

            // nombre de proposition d'habillage faite
            // la note moyenne
            var q2 = this.ListWearingMade(context, personne.Id, style);
            obj.NumberB = q2.Count();
            if (obj.NumberB == 0)
                obj.Note = "0//0";
            else
                obj.Note = string.Format("{0}//{1}",
                        q2.Where(x => x != null).Select(x => x.Note).Average(c => c.Value).ToString(),
                        q2.Count().ToString());

            // nombre de photo habillé
            obj.NumberC = this.CountWearingPictures(context, personne.Id, style);

            return obj;
        }
        private int CountReceiveDemands(ConseilEntitiesBis context, int personne, int style)
        {
            return (from c in context.Conseils
                    where c.ConseillerId == personne &&
                    (c.TypeId == (int)DemandeStatus.AttenteConseiller ||
                     c.TypeId == (int)DemandeStatus.RefusConseiller ||
                     c.TypeId == (int)DemandeStatus.Termine) &&
                    c.StyleId == style
                    select new { c.Id }).Count();
        }
        private List<Habillage> ListWearingMade(ConseilEntitiesBis context, int personne, int style)
        {
            return (from c in context.Conseils
                    from h in context.Habillages
                       .Where(o => c.Id == o.ConseilId)
                       .DefaultIfEmpty()
                    where c.ConseillerId == personne &&
                          (c.TypeId == (int)DemandeStatus.Accepte || c.TypeId == (int)DemandeStatus.Termine) &&
                          c.StyleId == style
                    select h).ToList();
        }
        private int CountWearPictures(ConseilEntitiesBis context, int personne, int style)
        {
            return (from ph in context.Photos.Include("Styles")
                    where ph.PersonneId == personne &&
                          ph.TypeId == (int)PhotoType.Vetement &&
                          ph.VetementId != null &&
                          ph.Styles.Count(s => s.Id == style) > 0
                    select new { ph.Id }).Count();
        }
        private int CountWearingPictures(ConseilEntitiesBis context, int personne, int style)
        {
            return (from ph in context.Photos.Include("Styles")
                    where ph.PersonneId == personne &&
                          ph.TypeId == (int)PhotoType.Habille &&
                          ph.Styles.Count(s => s.Id == style) > 0
                    select new { ph.Id }).Count();
        }
        private PersonViewItem GetSubscriberInfos(ConseilEntitiesBis context, PersonViewItem personne, int style)
        {
            var obj = new PersonViewItem();
            obj.Id = personne.Id;
            obj.Login = personne.Login;
            obj.Genre = personne.Genre;

            // nombre de photo de vêtement
            obj.NumberA = this.CountWearPictures(context, personne.Id, style);

            // nombre de photo habillé
            obj.NumberB = this.CountWearingPictures(context, personne.Id, style);
                        
            return obj;
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

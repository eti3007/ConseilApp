using ConseilOBJ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConseilApp.Controllers
{
    public class BaseController : Controller
    {
        #region Properties
        public int PersonneId
        {
            get { try {
                return GetSession<int>(SessionKey.PersonneID);
            }
            catch { return 0; }            
            }
        }
        public int StyleEnCours
        {
            get { try {
                return Convert.ToInt32(GetCookies(Enum.GetName(typeof(CookieKey), CookieKey.style)));
            }
            catch { return 0; }
            }
        }
        public string PersonnePseudo
        {
            get
            {
                string CookieName = Enum.GetName(typeof(CookieKey), CookieKey.pseudo);
                return GetCookies(CookieName);
            }
        }
        public List<Style> ListeDesStyles
        {
            get { try { 
                return GetSession<List<Style>>(SessionKey.ListeStyle);
            }
            catch { return new List<Style>(); }
            }
        }
        #endregion

        public void SetSession(SessionKey key, object data)
        {
            Session.Add(key.ToString(), data);
        }

        public T GetSession<T>(SessionKey key)
        {
            try
            {
                return (T)Session[key.ToString()];
            }
            catch { return default(T); }
        }

        public bool IsSetSession(SessionKey key)
        {
            return Session[key.ToString()] != null;
        }

        public void RemoveSession(SessionKey key)
        {
            Session.Remove(key.ToString());
        }

        public enum SessionKey
        {
            ListeStyle = 1,
            PersonneID,
            PersonnePseudo,
            PersonneStatut
        }

        [FlagsAttribute]
        public enum CookieKey
        {
            pseudo,
            style,
            menuRecherche
        }

        public enum mnuIdentifiant    // TODO : vérifier son utilisation ainsi que les cookies !!!!!
        {
            MesInfos=10,
            MesPhotos=20,
            MesDemandes=30,
            MesPropositions=40
        }

        /// <summary>
        /// Cette méthode permet de retourner une liste d'item pour les DDL
        /// uniquement pour les données de paramètres
        /// </summary>
        /// <param name="obj">Liste typé</param>
        /// <returns>liste de SelectListItem</returns>
        public List<SelectListItem> DDLTypeParam(List<TypeParam> obj)
        {
            List<SelectListItem> objListe = new List<SelectListItem>();
            SelectListItem itemList;

            foreach (var item in obj)
            {
                itemList = new SelectListItem();
                itemList.Text = item.ParamLib;
                itemList.Value = item.Id.ToString();
                objListe.Add(itemList);
            }
            return objListe;
        }

        public void SetCookies(string CookieName, string CookieValue)
        {
            var cookie = new HttpCookie(CookieName, CookieValue);
            Response.AppendCookie(cookie);
        }

        public string GetCookies(string CookieName)
        {
            if (Request.Cookies.AllKeys.Count() == 0) return string.Empty;
            if (Request.Cookies[CookieName] == null) return string.Empty;
            return !string.IsNullOrEmpty(Request.Cookies[CookieName].Value) ? Request.Cookies[CookieName].Value : string.Empty;
        }

        /// <summary>
        /// Authentifie la personne et garde certaine information en session et dans les cookies
        /// </summary>
        /// <param name="id">identifiant de la personne</param>
        /// <param name="pseudo">pseudo de la personne</param>
        /// <param name="styleId">le style par defaut</param>
        public void RegisterPerson(int id, string pseudo, string styleId, bool replaceStyle=true)
        {
            if (replaceStyle || !IsSetSession(SessionKey.PersonneID)) SetSession(SessionKey.PersonneID, id);

            SetCookies(CookieKey.pseudo.ToString(), pseudo);
            if (replaceStyle || this.StyleEnCours == 0) SetCookies(CookieKey.style.ToString(), styleId);
        }

        public void UpdateStyleCookieValue(string styleId)
        {
            SetCookies(CookieKey.style.ToString(), styleId);
        }

        public void MenuDemandeEnregistreCookie()
        { SetCookies(CookieKey.menuRecherche.ToString(), mnuIdentifiant.MesDemandes.ToString()); }
        public void MenuPropositionEnregistreCookie()
        { SetCookies(CookieKey.menuRecherche.ToString(), mnuIdentifiant.MesPropositions.ToString()); }
        public bool? IsRechercheDemandeMenu()
        {
            if (GetCookies(CookieKey.menuRecherche.ToString()) == mnuIdentifiant.MesDemandes.ToString())
            { return true; }
            else if (GetCookies(CookieKey.menuRecherche.ToString()) == mnuIdentifiant.MesPropositions.ToString())
            { return false; }
            else
            { return null; }
        }

        // méthode appelé lors du logOut :
        public void ResetSession()
        {
            RemoveSession(SessionKey.ListeStyle);
            RemoveSession(SessionKey.PersonneID);
            RemoveSession(SessionKey.PersonnePseudo);
            RemoveSession(SessionKey.PersonneStatut);

        }
    }
}

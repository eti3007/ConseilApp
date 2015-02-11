using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;
using ConseilOBJ;

namespace ConseilApp.Models
{
    public class AccountViewModel : RegisterModel
    {
        private Personne _pers;

        public Personne Personne_inside
        {
            get { return _pers; }
        }

        public AccountViewModel(Personne pers)
        {
            _pers = pers;
        }
        public AccountViewModel()
        {
            _pers = new Personne();
        }

        public int Id { get { return _pers.Id; } set { _pers.Id = value; } }
        public string FBCode { get { return _pers.FBCode; } set { _pers.FBCode = value; } }
        public string Nom { get { return _pers.Nom; } set { _pers.Nom = value; } }
        public string Prenom { get { return _pers.Prenom; } set { _pers.Prenom = value; } }
        public string Pseudo { get { return _pers.Pseudo; } set { _pers.Pseudo = value; } }

        [Display(Name = "Adresse mail")]
        [DataType(DataType.EmailAddress)]
        public string Email { get { return _pers.Email; } set { _pers.Email = value; } }

        [Display(Name="Êtes-vous un homme ou une femme ?")]
        public string Genre { get { return _pers.Genre; } set { _pers.Genre = value; } }
        public IEnumerable<System.Web.Mvc.SelectListItem> genreListe { get; set; }

        public bool AfficheInfo { get { return _pers.AfficheInfo.HasValue ? _pers.AfficheInfo.Value : false; } set { _pers.AfficheInfo = value; } }

        [Display(Name = "Recevoir les nouvelles du site sur votre adresse mail ?")]
        public bool EnvoiEmail { get { return _pers.EnvoiEmail.HasValue ? _pers.EnvoiEmail.Value : false; } set { _pers.EnvoiEmail = value; } }
        public string MotPasse { get { return _pers.MotPasse; } set { _pers.MotPasse = value; } }

        [Display(Name = "Votre style par défaut")]
        [Required(ErrorMessage = "Veuillez choisir un style")]
        public string Style { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> styleListe { get; set; }
    }
}
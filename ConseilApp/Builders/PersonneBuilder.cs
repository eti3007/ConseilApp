using ConseilOBJ;
using ConseilApp.Models;
using ConseilApp.Builders.Interfaces;

namespace ConseilApp.Builders
{
    public class PersonneBuilder : IPersonneBuilder
    {
        /// <summary>
        /// Méthode utilisée pour créer un objet Personne depuis le ViewModel de la page Register
        /// </summary>
        public Personne PersonneToRegister(AccountViewModel model)
        {
            return new Personne()
            {
                Pseudo = model.UserName,
                Email = model.Email,
                EnvoiEmail = model.EnvoiEmail,
                Genre = model.Genre
            };
        }
    }
}
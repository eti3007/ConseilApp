using ConseilOBJ;
using ConseilApp.Models;

namespace ConseilApp.Builders.Interfaces
{
    public interface IPersonneBuilder
    {
        Personne PersonneToRegister(AccountViewModel model);
    }
}

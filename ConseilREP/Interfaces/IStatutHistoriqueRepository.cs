using ConseilOBJ;

namespace ConseilREP.Interfaces
{
    public interface IStatutHistoriqueRepository
    {
        int GetPersonStyleStatus(int personneId, int styleId);
    }
}

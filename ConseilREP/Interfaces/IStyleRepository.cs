using System.Collections.Generic;
using ConseilOBJ;

namespace ConseilREP.Interfaces
{
    public interface IStyleRepository
    {
        List<Style> GetList();
        Style GetById(int styleId);
        List<Style> GetListForHabillage(int persId);
    }
}

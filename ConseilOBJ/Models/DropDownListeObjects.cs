using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConseilOBJ
{
    public class DropDownListeVetement : IDropDownListeObject
    {
        private int _Id;
        public int Id
        { get { return _Id; } set { _Id = value; } }

        private string _Nom;
        public string Nom
        { get { return _Nom; } set { _Nom = value; } }
    }

    public class DropDownListeTypeParam : IDropDownListeObject
    {
        private int _Id;
        public int Id
        { get { return _Id; } set { _Id = value; } }

        private string _Nom;
        public string Nom
        { get { return _Nom; } set { _Nom = value; } }
    }

    public class DropDownListeStyle : IDropDownListeObject
    {
        private int _Id;
        public int Id
        { get { return _Id; } set { _Id = value; } }

        private string _Nom;
        public string Nom
        { get { return _Nom; } set { _Nom = value; } }
    }
}

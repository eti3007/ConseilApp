//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ConseilOBJ
{
    using System;
    using System.Collections.Generic;
    
    public partial class Habillage
    {
        public Habillage()
        {
            this.Photos = new HashSet<Photo>();
        }
    
        public int Id { get; set; }
        public System.DateTime DateCreation { get; set; }
        public string Commentaire { get; set; }
        public Nullable<short> Note { get; set; }
        public int ConseilId { get; set; }
    
        public virtual Conseil Conseil { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
    }
}

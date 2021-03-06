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
    
    public partial class Photo
    {
        public Photo()
        {
            this.Habillages = new HashSet<Habillage>();
            this.Styles = new HashSet<Style>();
        }
    
        public int Id { get; set; }
        public string Url { get; set; }
        public int TypeId { get; set; }
        public Nullable<int> VetementId { get; set; }
        public int PersonneId { get; set; }
        public System.DateTime DateTelecharge { get; set; }
    
        public virtual Personne Personne { get; set; }
        public virtual TypeParam TypeParam { get; set; }
        public virtual Vetement Vetement { get; set; }
        public virtual ICollection<Habillage> Habillages { get; set; }
        public virtual ICollection<Style> Styles { get; set; }
    }
}

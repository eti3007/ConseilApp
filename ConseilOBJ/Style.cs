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
    
    public partial class Style
    {
        public Style()
        {
            this.Conseils = new HashSet<Conseil>();
            this.StatutHistoriques = new HashSet<StatutHistorique>();
            this.Photos = new HashSet<Photo>();
        }
    
        public int Id { get; set; }
        public string Nom { get; set; }
    
        public virtual ICollection<Conseil> Conseils { get; set; }
        public virtual ICollection<StatutHistorique> StatutHistoriques { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
    }
}

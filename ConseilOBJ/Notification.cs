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
    
    public partial class Notification
    {
        public int Id { get; set; }
        public System.DateTime DateCreation { get; set; }
        public string Message { get; set; }
        public int TypeId { get; set; }
        public Nullable<int> PersonneId { get; set; }
        public Nullable<int> ConseilId { get; set; }
    
        public virtual Conseil Conseil { get; set; }
        public virtual Personne Personne { get; set; }
        public virtual TypeParam TypeParam { get; set; }
    }
}

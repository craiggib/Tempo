//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TEMPO.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Quote
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Quote()
        {
            this.projects = new HashSet<Project>();
        }
    
        public int quoteid { get; set; }
        public double hours { get; set; }
        public string description { get; set; }
        public decimal price { get; set; }
        public Nullable<int> clientid { get; set; }
    
        public virtual Client client { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Project> projects { get; set; }
    }
}
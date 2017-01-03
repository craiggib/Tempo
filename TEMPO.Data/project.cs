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
    
    public partial class Project
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Project()
        {
            this.mmts = new HashSet<MiscTimeLog>();
            this.timeentries = new HashSet<TimeEntry>();
        }
    
        public int projectid { get; set; }
        public Nullable<int> clientid { get; set; }
        public int jobnumyear { get; set; }
        public string jobnum { get; set; }
        public string refjobnum { get; set; }
        public Nullable<int> projecttypeid { get; set; }
        public string description { get; set; }
        public bool Active { get; set; }
        public Nullable<int> quoteid { get; set; }
        public Nullable<decimal> contractamount { get; set; }
        public Nullable<int> Weight { get; set; }
        public Nullable<int> DrawingCount { get; set; }
    
        public virtual Client client { get; set; }
        public virtual JobYear JobYear { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MiscTimeLog> mmts { get; set; }
        public virtual ProjectType projecttype { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TimeEntry> timeentries { get; set; }
        public virtual Quote quote { get; set; }
    }
}

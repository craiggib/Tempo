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
    
    public partial class TimeSheet
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TimeSheet()
        {
            this.timeentries = new HashSet<TimeEntry>();
        }
    
        public int tid { get; set; }
        public Nullable<int> peid { get; set; }
        public Nullable<int> empid { get; set; }
        public Nullable<int> statusid { get; set; }
        public string notes { get; set; }
    
        public virtual Employee employee { get; set; }
        public virtual PeriodEnding periodending { get; set; }
        public virtual Status status { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TimeEntry> timeentries { get; set; }
    }
}

namespace TEMPO.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("timesheet")]
    public partial class TimeSheet
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TimeSheet()
        {
            timeentries = new HashSet<TimeEntry>();
        }

        [Key]
        public int tid { get; set; }

        public int? peid { get; set; }

        public int? empid { get; set; }

        public int? statusid { get; set; }

        [StringLength(500)]
        public string notes { get; set; }

        [StringLength(500)]
        public string approvalnotes { get; set; }

        public virtual Employee employee { get; set; }

        public virtual PeriodEnding periodending { get; set; }

        public virtual Status status { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TimeEntry> timeentries { get; set; }
    }
}

namespace TEMPO.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("worktype")]
    public partial class WorkType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public WorkType()
        {           
            timeentries = new HashSet<TimeEntry>();
        }

        public int worktypeid { get; set; }

        [Required]
        [StringLength(50)]
        public string worktypename { get; set; }
       

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TimeEntry> timeentries { get; set; }
    }
}

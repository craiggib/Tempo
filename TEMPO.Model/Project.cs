namespace TEMPO.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("project")]
    public partial class Project
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Project()
        {            
            timeentries = new HashSet<TimeEntry>();
        }

        public int projectid { get; set; }

        public int? clientid { get; set; }

        public int jobnumyear { get; set; }

        [Required]
        [StringLength(10)]
        public string jobnum { get; set; }

        [StringLength(50)]
        public string refjobnum { get; set; }

        public int? projecttypeid { get; set; }

        [StringLength(30)]
        public string description { get; set; }

        public bool Active { get; set; }

        public int? quoteid { get; set; }

        public decimal? contractamount { get; set; }

        public int? Weight { get; set; }

        public int? DrawingCount { get; set; }

        public virtual Client client { get; set; }

        public virtual JobYear JobYear { get; set; }
                
        public virtual ProjectType projecttype { get; set; }

        public virtual Quote quote { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TimeEntry> timeentries { get; set; }
    }
}

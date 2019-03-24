namespace TEMPO.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("quote")]
    public partial class Quote
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Quote()
        {
            projects = new HashSet<Project>();
            quotetags = new HashSet<QuoteTag>();
        }

        public int quoteid { get; set; }

        public double hours { get; set; }

        [StringLength(500)]
        public string description { get; set; }

        public decimal price { get; set; }

        public int? clientid { get; set; }

        [StringLength(250)]
        public string clientname { get; set; }

        public DateTime createddate { get; set; }

        public DateTime lastupdateddate { get; set; }

        public int createdby { get; set; }

        public bool awarded { get; set; }

        public DateTime? awardedDate { get; set; }

        public virtual Client client { get; set; }

        public virtual Employee employee { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Project> projects { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QuoteTag> quotetags { get; set; }
    }
}

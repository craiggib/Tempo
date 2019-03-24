namespace TEMPO.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProjectSummary")]
    public partial class ProjectSummary
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int projectid { get; set; }

        public int? clientid { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string jobnum { get; set; }

        [StringLength(50)]
        public string refjobnum { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string projecttypedesc { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(255)]
        public string clientname { get; set; }

        [StringLength(30)]
        public string description { get; set; }

        [Key]
        [Column(Order = 4)]
        public bool active { get; set; }

        [Key]
        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int JobYear { get; set; }

        public decimal? InternalAmount { get; set; }

        public decimal? TotalHours { get; set; }

        public DateTime? lastHoursLogged { get; set; }

        public decimal? contractamount { get; set; }
    }
}

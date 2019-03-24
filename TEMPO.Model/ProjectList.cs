namespace TEMPO.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProjectList")]
    public partial class ProjectList
    {
        [StringLength(44)]
        public string ProjectName { get; set; }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int projectid { get; set; }

        public int? clientid { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int jobnumyear { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(10)]
        public string jobnum { get; set; }

        [StringLength(50)]
        public string refjobnum { get; set; }

        public int? projecttypeid { get; set; }

        [StringLength(30)]
        public string description { get; set; }
    }
}

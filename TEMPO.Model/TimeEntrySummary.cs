namespace TEMPO.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TimeEntrySummary")]
    public partial class TimeEntrySummary
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int entryid { get; set; }

        public int? projectid { get; set; }

        public decimal? entryHours { get; set; }

        public decimal? internalamount { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(255)]
        public string employeename { get; set; }

        [Key]
        [Column(Order = 2)]
        public DateTime endingdate { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int tid { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(50)]
        public string worktypename { get; set; }

        [Key]
        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int worktypeid { get; set; }
    }
}

namespace TEMPO.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("timeentry")]
    public partial class TimeEntry
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TimeEntry()
        {
        }

        [Key]
        public int entryid { get; set; }

        public int? tid { get; set; }

        public decimal? sunday { get; set; }

        public decimal? monday { get; set; }

        public decimal? tuesday { get; set; }

        public decimal? wednesday { get; set; }

        public decimal? thursday { get; set; }

        public decimal? friday { get; set; }

        public decimal? saturday { get; set; }

        public int? worktypeid { get; set; }

        public int? projectid { get; set; }


        public virtual Project project { get; set; }

        public virtual TimeSheet timesheet { get; set; }

        public virtual WorkType worktype { get; set; }
    }
}

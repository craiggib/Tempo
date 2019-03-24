namespace TEMPO.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("mmt")]
    public partial class Mmt
    {
        public int mmtid { get; set; }

        public int? entryid { get; set; }

        public int? worktypeid { get; set; }

        public int? projectid { get; set; }

        public virtual TimeEntry timeentry { get; set; }

        public virtual Project project { get; set; }

        public virtual WorkType worktype { get; set; }
    }
}

namespace TEMPO.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class QuoteTag
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int quoteid { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(100)]
        public string title { get; set; }

        public virtual Quote quote { get; set; }
    }
}

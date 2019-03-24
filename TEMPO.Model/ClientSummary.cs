namespace TEMPO.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ClientSummary")]
    public partial class ClientSummary
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int clientid { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(255)]
        public string clientname { get; set; }

        public int? projectcount { get; set; }

        public DateTime? lastHoursLogged { get; set; }

        public decimal? totalhourslogged { get; set; }

        public decimal? internaltotalamount { get; set; }

        public decimal? TotalContractedAmount { get; set; }
    }
}

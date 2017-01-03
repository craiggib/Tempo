using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TEMPO.WebApp.Models.Client
{
    public class ClientSummary
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public int ProjectCount { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime? LastHoursLogged { get; set; }
        public float TotalHoursLogged { get; set; }

        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:c}")]
        public float TotalInternalAmount { get; set; }
    }
}
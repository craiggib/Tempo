//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TEMPO.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class ClientSummary
    {
        public int clientid { get; set; }
        public string clientname { get; set; }
        public Nullable<int> projectcount { get; set; }
        public Nullable<System.DateTime> lastHoursLogged { get; set; }
        public Nullable<decimal> totalhourslogged { get; set; }
        public Nullable<decimal> internaltotalamount { get; set; }
    }
}

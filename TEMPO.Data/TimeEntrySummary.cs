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
    
    public partial class TimeEntrySummary
    {
        public int entryid { get; set; }
        public Nullable<int> projectid { get; set; }
        public Nullable<decimal> entryHours { get; set; }
        public Nullable<decimal> internalamount { get; set; }
        public string employeename { get; set; }
        public System.DateTime endingdate { get; set; }
        public int tid { get; set; }
        public string worktypename { get; set; }
    }
}
﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class TEMPOEntities : DbContext
    {
        public TEMPOEntities()
            : base("name=TEMPOEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<client> clients { get; set; }
        public virtual DbSet<employee> employees { get; set; }
        public virtual DbSet<JobYear> JobYears { get; set; }
        public virtual DbSet<mmt> mmts { get; set; }
        public virtual DbSet<module> modules { get; set; }
        public virtual DbSet<periodending> periodendings { get; set; }
        public virtual DbSet<project> projects { get; set; }
        public virtual DbSet<projecttype> projecttypes { get; set; }
        public virtual DbSet<status> status { get; set; }
        public virtual DbSet<timeentry> timeentries { get; set; }
        public virtual DbSet<timesheet> timesheets { get; set; }
        public virtual DbSet<worktype> worktypes { get; set; }
    }
}

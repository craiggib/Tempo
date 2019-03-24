namespace TEMPO.Model
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class TempoContext : DbContext
    {
        public TempoContext()
            : base("name=TempoContext")
        {
        }

        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<JobYear> JobYears { get; set; }
        public virtual DbSet<Mmt> Mmts { get; set; }
        public virtual DbSet<Module> modules { get; set; }
        public virtual DbSet<PeriodEnding> PeriodEndings { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ProjectType> ProjectTypes { get; set; }
        public virtual DbSet<Quote> Quotes { get; set; }
        public virtual DbSet<QuoteTag> QuoteTags { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<TimeEntry> TimeEntries { get; set; }
        public virtual DbSet<TimeSheet> TimeSheets { get; set; }
        public virtual DbSet<WorkType> WorkTypes { get; set; }
        public virtual DbSet<ClientSummary> ClientSummaries { get; set; }
        public virtual DbSet<ProjectList> ProjectLists { get; set; }
        public virtual DbSet<ProjectSummary> ProjectSummaries { get; set; }
        public virtual DbSet<TimeEntrySummary> TimeEntrySummaries { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>()
                .Property(e => e.clientname)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.employeename)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.password)
                .IsUnicode(false);

            modelBuilder.Entity<Employee>()
                .Property(e => e.rate)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.quotes)
                .WithRequired(e => e.employee)
                .HasForeignKey(e => e.createdby)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.modules)
                .WithMany(e => e.employees)
                .Map(m => m.ToTable("moduleauth").MapLeftKey("empid").MapRightKey("moduleid"));

            modelBuilder.Entity<JobYear>()
                .HasMany(e => e.projects)
                .WithRequired(e => e.JobYear)
                .HasForeignKey(e => e.jobnumyear)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Module>()
                .Property(e => e.modulename)
                .IsUnicode(false);

            modelBuilder.Entity<Project>()
                .Property(e => e.jobnum)
                .IsUnicode(false);

            modelBuilder.Entity<Project>()
                .Property(e => e.refjobnum)
                .IsUnicode(false);

            modelBuilder.Entity<Project>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<Project>()
                .Property(e => e.contractamount)
                .HasPrecision(18, 0);

            modelBuilder.Entity<ProjectType>()
                .Property(e => e.projecttypedesc)
                .IsUnicode(false);

            modelBuilder.Entity<Quote>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<Quote>()
                .Property(e => e.price)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Quote>()
                .Property(e => e.clientname)
                .IsUnicode(false);

            modelBuilder.Entity<QuoteTag>()
                .Property(e => e.title)
                .IsUnicode(false);

            modelBuilder.Entity<Status>()
                .Property(e => e.statusname)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TimeEntry>()
                .Property(e => e.sunday)
                .HasPrecision(5, 2);

            modelBuilder.Entity<TimeEntry>()
                .Property(e => e.monday)
                .HasPrecision(5, 2);

            modelBuilder.Entity<TimeEntry>()
                .Property(e => e.tuesday)
                .HasPrecision(5, 2);

            modelBuilder.Entity<TimeEntry>()
                .Property(e => e.wednesday)
                .HasPrecision(5, 2);

            modelBuilder.Entity<TimeEntry>()
                .Property(e => e.thursday)
                .HasPrecision(5, 2);

            modelBuilder.Entity<TimeEntry>()
                .Property(e => e.friday)
                .HasPrecision(5, 2);

            modelBuilder.Entity<TimeEntry>()
                .Property(e => e.saturday)
                .HasPrecision(5, 2);

            modelBuilder.Entity<TimeEntry>()
                .HasMany(e => e.mmts)
                .WithOptional(e => e.timeentry)
                .WillCascadeOnDelete();

            modelBuilder.Entity<TimeSheet>()
                .Property(e => e.notes)
                .IsUnicode(false);

            modelBuilder.Entity<TimeSheet>()
                .Property(e => e.approvalnotes)
                .IsUnicode(false);

            modelBuilder.Entity<TimeSheet>()
                .HasMany(e => e.timeentries)
                .WithOptional(e => e.timesheet)
                .WillCascadeOnDelete();

            modelBuilder.Entity<WorkType>()
                .Property(e => e.worktypename)
                .IsUnicode(false);

            modelBuilder.Entity<ClientSummary>()
                .Property(e => e.clientname)
                .IsUnicode(false);

            modelBuilder.Entity<ClientSummary>()
                .Property(e => e.totalhourslogged)
                .HasPrecision(38, 2);

            modelBuilder.Entity<ClientSummary>()
                .Property(e => e.internaltotalamount)
                .HasPrecision(38, 4);

            modelBuilder.Entity<ClientSummary>()
                .Property(e => e.TotalContractedAmount)
                .HasPrecision(38, 4);

            modelBuilder.Entity<ProjectList>()
                .Property(e => e.ProjectName)
                .IsUnicode(false);

            modelBuilder.Entity<ProjectList>()
                .Property(e => e.jobnum)
                .IsUnicode(false);

            modelBuilder.Entity<ProjectList>()
                .Property(e => e.refjobnum)
                .IsUnicode(false);

            modelBuilder.Entity<ProjectList>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<ProjectSummary>()
                .Property(e => e.jobnum)
                .IsUnicode(false);

            modelBuilder.Entity<ProjectSummary>()
                .Property(e => e.refjobnum)
                .IsUnicode(false);

            modelBuilder.Entity<ProjectSummary>()
                .Property(e => e.projecttypedesc)
                .IsUnicode(false);

            modelBuilder.Entity<ProjectSummary>()
                .Property(e => e.clientname)
                .IsUnicode(false);

            modelBuilder.Entity<ProjectSummary>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<ProjectSummary>()
                .Property(e => e.InternalAmount)
                .HasPrecision(38, 4);

            modelBuilder.Entity<ProjectSummary>()
                .Property(e => e.TotalHours)
                .HasPrecision(38, 2);

            modelBuilder.Entity<ProjectSummary>()
                .Property(e => e.contractamount)
                .HasPrecision(38, 4);

            modelBuilder.Entity<TimeEntrySummary>()
                .Property(e => e.entryHours)
                .HasPrecision(38, 2);

            modelBuilder.Entity<TimeEntrySummary>()
                .Property(e => e.internalamount)
                .HasPrecision(38, 4);

            modelBuilder.Entity<TimeEntrySummary>()
                .Property(e => e.employeename)
                .IsUnicode(false);

            modelBuilder.Entity<TimeEntrySummary>()
                .Property(e => e.worktypename)
                .IsUnicode(false);
        }
    }
}

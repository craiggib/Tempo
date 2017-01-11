using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TEMPO.Data;

namespace TEMPO.WebApp.Controllers
{
    public class BaseController : Controller
    {
        protected const string USERID_COOKIE_NAME = "Tempo.UserId";
        private IMapper _mapper;

        protected IMapper Mapper
        {
            get
            {
                return _mapper;
            }
        }

        public BaseController()
        {

            var config = new MapperConfiguration(i =>
            {
                i.CreateMap<Data.TimeSheet, Models.Timesheet.Timesheet>()
                    .ForMember(d => d.PeriodEnding, o => o.MapFrom(s => s.periodending.endingdate))
                    .ForMember(d => d.StatusName, o => o.MapFrom(s => s.status.statusname.Trim()))
                    .ForMember(d => d.TimesheetId, o => o.MapFrom(s => s.tid))
                    .ForMember(d => d.EmployeeName, o => o.MapFrom(s => s.employee.employeename))
                    .AfterMap((src, dest) => dest.WeeklyTotal = dest.TimeEntries.Sum(j => j.Sunday + j.Monday + j.Tuesday + j.Wednesday + j.Thursday + j.Friday + j.Saturday));

                i.CreateMap<Data.TimeEntry, Models.Timesheet.TimeEntry>()
                    .ForMember(d => d.ProjectName, o => o.MapFrom(s => $"{s.project.JobYear.JobYear1}-{s.project.jobnum} {s.project.description}"))
                    .ForMember(d => d.WorkTypeName, o => o.MapFrom(s => s.worktype.worktypename));

                i.CreateMap<Data.TimeEntrySummary, Models.Timesheet.TimeEntrySummary>()
                    .ForMember(d => d.TimesheetId, o => o.MapFrom(s => s.tid));

                i.CreateMap<Data.WorkType, Models.Timesheet.WorkType>();

                i.CreateMap<Data.PeriodEnding, Models.Timesheet.PeriodEnding>()
                    .ForMember(d => d.PeriodEndingId, o => o.MapFrom(s => s.peid));

                i.CreateMap<Data.Client, Models.Client.Client>();
                i.CreateMap<Data.ClientSummary, Models.Client.ClientSummary>()
                    .ForMember(d => d.TotalInternalAmount, o => o.MapFrom(s => s.internaltotalamount));

                i.CreateMap<Data.Quote, Models.Quote.Quote>()
                    .ForMember(d => d.EstimatedHours, o => o.MapFrom(s => s.hours))
                    .ForMember(d => d.EstimatedPrice, o => o.MapFrom(s => s.price))
                    .ForMember(d => d.CreatedBy, o => o.MapFrom(s => s.employee.employeename))
                    .ForMember(d => d.ClientName, o => o.MapFrom(s => s.clientid.HasValue ? s.client.clientname : s.clientname))
                    .AfterMap((src, dest) => dest.Tags = string.Join(",", src.quotetags.Select(j => j.title)));

                i.CreateMap<Data.QuoteTagFrequency, Models.Quote.QuoteTagFrequency>();

                i.CreateMap<Data.JobYear, Models.Project.JobYear>()
                    .ForMember(d => d.Year, o => o.MapFrom(s => s.JobYear1));

                i.CreateMap<Data.ProjectType, Models.Project.ProjectType>()
                    .ForMember(d => d.Description, o => o.MapFrom(s => s.projecttypedesc));

                i.CreateMap<Data.Project, Models.Project.Project>()
                    .ForMember(d => d.ProjectId, o => o.MapFrom(s => s.projectid))
                    .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.client.clientname))
                    .ForMember(d => d.ProjectNumber, o => o.MapFrom(s => s.jobnum))
                    .ForMember(d => d.ReferenceJobNumber, o => o.MapFrom(s => s.refjobnum))
                    .ForMember(d => d.ContractedAmount, o => o.MapFrom(s => s.contractamount))
                    .ForMember(d => d.JobYearId, o => o.MapFrom(s => s.jobnumyear))
                    .ForMember(d => d.ProjectName, o => o.MapFrom(s => $"{s.JobYear.JobYear1}-{s.jobnum} {s.description}"));
                   
                i.CreateMap<Data.ProjectSummary, Models.Project.ProjectSummary>()
                    .ForMember(d => d.ReferenceJobNumber, o => o.MapFrom(s => s.refjobnum))
                    .ForMember(d => d.ProjectType, o => o.MapFrom(s => s.projecttypedesc))
                    .ForMember(d => d.JobNumber, o => o.MapFrom(s => s.jobnum))
                    .ForMember(d => d.ContractedAmount, o => o.MapFrom(s => s.contractamount))                    
                    .ForMember(d => d.ProjectName, o => o.MapFrom(s => $"{s.JobYear}-{s.jobnum} {s.description}"));

                i.CreateMap<Data.Employee, Models.Employee.Employee>()
                    .ForMember(d => d.EmployeeId, o => o.MapFrom(s => s.empid));

            });

            _mapper = config.CreateMapper();
        }

        protected int GetUserID()
        {
            HttpCookie myCookie = Request.Cookies[USERID_COOKIE_NAME];
            if (myCookie != null)
            {
                return int.Parse(myCookie.Value);
            }
            else
            {
                return -1;
            }
        }

        
    }
}
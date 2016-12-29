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
        private const string USERID_COOKIE_NAME = "Tempo.UserId";
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
                    .ForMember(d => d.StatusName, o => o.MapFrom(s => s.status.statusname))
                    .ForMember(d => d.TimesheetId, o => o.MapFrom(s => s.tid));

                i.CreateMap<Data.TimeEntry, Models.Timesheet.TimeEntry>();

                i.CreateMap<Data.WorkType, Models.Timesheet.WorkType>();

                i.CreateMap<Data.Project, Models.Project.Project>()
                    .ForMember(d => d.ProjectId, o => o.MapFrom(s => s.projectid))
                    .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.client.clientname))
                    .ForMember(d => d.ReferenceJobNumber, o => o.MapFrom(s => s.refjobnum))
                    .ForMember(d => d.ProjectName, o => o.MapFrom(s => $"{s.JobYear.JobYear1}-{s.jobnum} {s.description}"));
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

        protected void SetUserId(Employee employee)
        {
            HttpCookie userIdCookie = new HttpCookie(USERID_COOKIE_NAME);
            userIdCookie.Value = employee.empid.ToString();
            userIdCookie.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Add(userIdCookie);
        }
    }
}
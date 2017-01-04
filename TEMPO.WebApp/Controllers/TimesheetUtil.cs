using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TEMPO.BusinessLayer.Project;
using TEMPO.BusinessLayer.TimeSheets;
using TEMPO.Data;

namespace TEMPO.WebApp.Controllers
{
    public class TimesheetUtil
    {
        private TimesheetManager _tsManager;
        private IMapper _mapper;

        public TimesheetUtil(IMapper mapper, TimesheetManager tsManager)
        {
            _tsManager = tsManager;
            _mapper = mapper;
        }

        public TimesheetUtil(IMapper mapper)
        {
            _tsManager = new TimesheetManager();
            _mapper = mapper;
        }

        public Models.Timesheet.Timesheet GetTimeSheet(int id)
        {            
            TimeSheet timesheet = _tsManager.GetTimeSheet(id);
            Models.Timesheet.Timesheet tsViewModel = _mapper.Map<Models.Timesheet.Timesheet>(timesheet);
            List<Models.Project.Project> projectList = BuildProjectList();
            List<Models.Timesheet.WorkType> workTypes = BuildWorkTypes();

            tsViewModel.TimeEntries.ForEach(i =>
            {
                i.Projects = new SelectList(projectList, "ProjectId", "ProjectName", i.ProjectId);
                i.WorkTypes = new SelectList(workTypes, "WorkTypeId", "WorkTypeName", i.WorkTypeId);
            });
            return tsViewModel;
        }

        public List<Models.Project.Project> BuildProjectList()
        {
            return new ProjectManager().GetProjects()
                            .Select(i => _mapper.Map<Models.Project.Project>(i))
                            .OrderByDescending(i => i.ProjectName)
                            .ToList();
        }

        public List<Models.Timesheet.WorkType> BuildWorkTypes()
        {
            return _tsManager.GetWorkTypes()
                .Select(i => _mapper.Map<Models.Timesheet.WorkType>(i))
                .OrderBy(i => i.WorkTypeName)
                .ToList();
        }

        public void UpdateTimesheet(Models.Timesheet.Timesheet timesheetVm)
        {
            foreach (var timeEntryVm in timesheetVm.TimeEntries)
            {
                if (timeEntryVm.EntryId == 0)
                {
                    _tsManager.AddTimeEntry(timesheetVm.TimesheetId, timeEntryVm.ProjectId, timeEntryVm.WorkTypeId, BuildDailyTime(timeEntryVm));
                }
                else
                {
                    _tsManager.UpdateTimeEntry(timeEntryVm.EntryId, timeEntryVm.ProjectId, timeEntryVm.WorkTypeId, BuildDailyTime(timeEntryVm));
                }
            }
        }

        private List<DailyTime> BuildDailyTime(Models.Timesheet.TimeEntry timeEntry)
        {
            var dailyTime = new List<DailyTime>();
            dailyTime.Add(timeEntry.Sunday != 0 ? new DailyTime { DayOfWeek = DayOfWeek.Sunday, HoursWorked = timeEntry.Sunday } : null);
            dailyTime.Add(timeEntry.Monday != 0 ? new DailyTime { DayOfWeek = DayOfWeek.Monday, HoursWorked = timeEntry.Monday } : null);
            dailyTime.Add(timeEntry.Tuesday != 0 ? new DailyTime { DayOfWeek = DayOfWeek.Tuesday, HoursWorked = timeEntry.Tuesday } : null);
            dailyTime.Add(timeEntry.Wednesday != 0 ? new DailyTime { DayOfWeek = DayOfWeek.Wednesday, HoursWorked = timeEntry.Wednesday } : null);
            dailyTime.Add(timeEntry.Thursday != 0 ? new DailyTime { DayOfWeek = DayOfWeek.Thursday, HoursWorked = timeEntry.Thursday } : null);
            dailyTime.Add(timeEntry.Friday != 0 ? new DailyTime { DayOfWeek = DayOfWeek.Friday, HoursWorked = timeEntry.Friday } : null);
            dailyTime.Add(timeEntry.Saturday != 0 ? new DailyTime { DayOfWeek = DayOfWeek.Saturday, HoursWorked = timeEntry.Saturday } : null);
            return dailyTime.Where(i => i != null).ToList();
        }
    }
}
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
    [Authorize]
    public class TimesheetController : BaseController
    {
        // GET: Timesheet
        public ActionResult Index(List<int> types)
        {
            var statusTypes = new List<TimesheetStatus>();

            if (types == null)
            {
                statusTypes.Add(TimesheetStatus.Approved);
                statusTypes.Add(TimesheetStatus.Rejected);
                statusTypes.Add(TimesheetStatus.Saved);
                statusTypes.Add(TimesheetStatus.Submitted);
            }
            else
            {
                foreach (int type in types)
                {
                    statusTypes.Add((TimesheetStatus)type);
                }
            }

            TimesheetManager tsManager = new TimesheetManager();
            List<TimeSheet> timesheets = tsManager.GetTimeSheets(GetUserID(), statusTypes);

            List<Models.Timesheet.Timesheet> timesheetVM = timesheets.Select(i =>
                Mapper.Map<Models.Timesheet.Timesheet>(i)).ToList();

            return View(timesheetVM.OrderByDescending(i => i.PeriodEnding));
        }

        [HttpPost]
        public void Create(Models.Timesheet.Timesheet timeSheet)
        {
            var t = timeSheet;
        }


        [HttpPost]
        public ActionResult Edit(Models.Timesheet.Timesheet timesheetVm)
        {
            TimesheetManager tsManager = new TimesheetManager();
            foreach (var timeEntryVm in timesheetVm.TimeEntries)
            {
                if(timeEntryVm.EntryId == 0)
                {
                    tsManager.AddTimeEntry(timesheetVm.TimesheetId, timeEntryVm.ProjectId, timeEntryVm.WorkTypeId, BuildDailyTime(timeEntryVm));
                }                
                else
                {
                    tsManager.UpdateTimeEntry(timeEntryVm.EntryId, timeEntryVm.ProjectId, timeEntryVm.WorkTypeId, BuildDailyTime(timeEntryVm));
                }
            }

            ViewBag.SuccessMessage = "Timesheet Saved";
            Models.Timesheet.Timesheet tsViewModel = GetTimeSheet(timesheetVm.TimesheetId, tsManager);
            return View(tsViewModel);
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

        public ActionResult Edit(int id)
        {
            TimesheetManager tsManager = new TimesheetManager();
            Models.Timesheet.Timesheet tsViewModel = GetTimeSheet(id, tsManager);

            return View(tsViewModel);
        }

        private Models.Timesheet.Timesheet GetTimeSheet(int id, TimesheetManager tsManager)
        {
            TimeSheet timesheet = tsManager.GetTimeSheet(id);
            Models.Timesheet.Timesheet tsViewModel = Mapper.Map<Models.Timesheet.Timesheet>(timesheet);
            List<Models.Project.Project> projectList = BuildProjectList();
            List<Models.Timesheet.WorkType> workTypes = BuildWorkTypes();

            tsViewModel.TimeEntries.ForEach(i =>
            {
                i.Projects = new SelectList(projectList, "ProjectId", "ProjectName", i.ProjectId);
                i.WorkTypes = new SelectList(workTypes, "WorkTypeId", "WorkTypeName", i.WorkTypeId);      
            });
            tsViewModel.WeeklyTotal = tsViewModel.TimeEntries.Sum(i => i.Sunday + i.Monday + i.Tuesday + i.Wednesday + i.Thursday + i.Friday + i.Saturday);
            return tsViewModel;
        }

        private List<Models.Project.Project> BuildProjectList()
        {
            return new ProjectManager().GetProjects()
                            .Select(i => Mapper.Map<Models.Project.Project>(i))
                            .OrderByDescending(i => i.ProjectName)
                            .ToList();
        }

        private List<Models.Timesheet.WorkType> BuildWorkTypes()
        {
            return new TimesheetManager().GetWorkTypes()
                            .Select(i => Mapper.Map<Models.Timesheet.WorkType>(i))
                            .OrderBy(i => i.WorkTypeName)
                            .ToList();
        }

        public PartialViewResult AddTimeEntry()
        {
            Models.Timesheet.TimeEntry teViewModel = new Models.Timesheet.TimeEntry();
            teViewModel.Projects = new SelectList(BuildProjectList(), "ProjectId", "ProjectName");
            teViewModel.WorkTypes = new SelectList(BuildWorkTypes(), "WorkTypeId", "WorkTypeName");
            return PartialView("_TimeEntry", teViewModel);
        }

        [HttpPost]
        public void DeleteTimeEntry(int entryId)
        {
            new TimesheetManager().DeleteTimeEntry(entryId);
        }
    }
}
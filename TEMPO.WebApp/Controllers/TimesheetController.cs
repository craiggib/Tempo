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
        private TimesheetManager _tsManager;

        public TimesheetController()
        {
            _tsManager = new TimesheetManager();
        }

        public ActionResult Index()
        {
            Models.Timesheet.TimesheetsHome timesheetsHomeVm = new Models.Timesheet.TimesheetsHome();

            timesheetsHomeVm.SavedTimeSheets = _tsManager
                .GetTimeSheets(GetUserID(), new List<TimesheetStatus> { TimesheetStatus.Saved })
                .Select(i => Mapper.Map<Models.Timesheet.Timesheet>(i))
                .OrderByDescending(i=>i.PeriodEnding)
                .ToList();

            timesheetsHomeVm.RejectedTimeSheets = _tsManager
                .GetTimeSheets(GetUserID(), new List<TimesheetStatus> { TimesheetStatus.Rejected })
                .Select(i => Mapper.Map<Models.Timesheet.Timesheet>(i))
                .OrderByDescending(i => i.PeriodEnding)
                .ToList();

            DateTime dateFilter = new DateTime(DateTime.Today.AddMonths(-2).Year, DateTime.Today.AddMonths(-2).Month, 1);
            DateTime monthCurrent = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            DateTime monthBack1 = new DateTime(DateTime.Today.AddMonths(-1).Year, DateTime.Today.AddMonths(-1).Month, 1);
            DateTime monthBack2 = new DateTime(DateTime.Today.AddMonths(-2).Year, DateTime.Today.AddMonths(-2).Month, 1);

            var approvedTimeSheets = _tsManager
                .GetTimeSheets(GetUserID(), new List<TimesheetStatus> { TimesheetStatus.Approved}, dateFilter)
                .Select(i => Mapper.Map<Models.Timesheet.Timesheet>(i));

            timesheetsHomeVm.ApprovedCurrentMonth = approvedTimeSheets
                .Where(i => i.PeriodEnding.Month == monthCurrent.Month)
                .ToList();

            timesheetsHomeVm.ApprovedBackMonth1 = approvedTimeSheets
                .Where(i => i.PeriodEnding.Month == monthBack1.Month)
                .ToList();

            timesheetsHomeVm.ApprovedBackMonth2 = approvedTimeSheets
                .Where(i => i.PeriodEnding.Month == monthBack2.Month)
                .ToList();

            var newEndingDateSelectList = _tsManager.GetNewPeriodEndings(GetUserID())
                .Select(i => Mapper.Map<Models.Timesheet.PeriodEnding>(i))
                .Select(i => new SelectListItem
                {
                    Text = i.EndingDate.ToString("dd-MMM-yyyy"),
                    Value = i.PeriodEndingId.ToString()
                })
                .ToList();
            timesheetsHomeVm.NewTimesheets = new SelectList(newEndingDateSelectList, "Value", "Text");

            return View(timesheetsHomeVm);
        }

        public ActionResult Details(int id)
        {
            if (TempData["submitted"] != null)
            {
                ViewBag.SuccessMessage = "Timesheet Submitted";
                TempData.Remove("submitted");
            }
            Models.Timesheet.Timesheet timesheetVM = Mapper.Map<Models.Timesheet.Timesheet>(_tsManager.GetTimeSheet(id));
            return View(timesheetVM);
        }

        [HttpPost]
        public ActionResult Create(Models.Timesheet.TimesheetsHome timeSheetsHomeVm)
        {
            Data.TimeSheet newTimSheet = _tsManager.CreateTimesheet(GetUserID(), timeSheetsHomeVm.NewPeriodEndingId);
            return RedirectToAction("Edit", new { id = newTimSheet.tid });
        }

        [HttpPost]
        public ActionResult Edit(Models.Timesheet.Timesheet timesheetVm)
        {
            UpdateTimesheet(timesheetVm);
            if (timesheetVm.SubmitForApproval)
            {
                _tsManager.SetState(timesheetVm.TimesheetId, TimesheetStatus.Submitted, timesheetVm.Notes);
                TempData["submitted"] = true;
                return RedirectToAction("Details", new { id = timesheetVm.TimesheetId });
            }
            else
            {
                ViewBag.SuccessMessage = "Timesheet Saved";
                Models.Timesheet.Timesheet tsViewModel = GetTimeSheet(timesheetVm.TimesheetId);
                return View(tsViewModel);
            }
        }

        private void UpdateTimesheet(Models.Timesheet.Timesheet timesheetVm)
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

        public ActionResult Edit(int id)
        {
            Models.Timesheet.Timesheet tsViewModel = GetTimeSheet(id);
            //if (tsViewModel.StatusName != "Saved" || tsViewModel.StatusName != "Rejected")
            //{
            //    return RedirectToAction("Review", new { id = id });
            //}
            return View(tsViewModel);
        }

        private Models.Timesheet.Timesheet GetTimeSheet(int id)
        {
            TimeSheet timesheet = _tsManager.GetTimeSheet(id);
            Models.Timesheet.Timesheet tsViewModel = Mapper.Map<Models.Timesheet.Timesheet>(timesheet);
            List<Models.Project.Project> projectList = BuildProjectList();
            List<Models.Timesheet.WorkType> workTypes = BuildWorkTypes();

            tsViewModel.TimeEntries.ForEach(i =>
            {
                i.Projects = new SelectList(projectList, "ProjectId", "ProjectName", i.ProjectId);
                i.WorkTypes = new SelectList(workTypes, "WorkTypeId", "WorkTypeName", i.WorkTypeId);
            });
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
            return _tsManager.GetWorkTypes()
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
            _tsManager.DeleteTimeEntry(entryId);
        }

      
    }
}
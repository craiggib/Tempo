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
        private TimesheetUtil _tsUtil;

        public TimesheetController()
        {
            _tsManager = new TimesheetManager();
            _tsUtil = new TimesheetUtil(Mapper, _tsManager);
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
            _tsUtil.UpdateTimesheet(timesheetVm);
            if (timesheetVm.SubmitForApproval)
            {
                _tsManager.SetState(timesheetVm.TimesheetId, TimesheetStatus.Submitted, timesheetVm.Notes);
                TempData["submitted"] = true;
                return RedirectToAction("Details", new { id = timesheetVm.TimesheetId });
            }
            else
            {
                ViewBag.SuccessMessage = "Timesheet Saved";
                Models.Timesheet.Timesheet tsViewModel = _tsUtil.GetTimeSheet(timesheetVm.TimesheetId);
                return View(tsViewModel);
            }
        }

        public ActionResult Edit(int id)
        {
            Models.Timesheet.Timesheet tsViewModel = _tsUtil.GetTimeSheet(id);
            return View(tsViewModel);
        }
        
        public PartialViewResult AddTimeEntry()
        {
            Models.Timesheet.TimeEntry teViewModel = new Models.Timesheet.TimeEntry();

            teViewModel.Projects = new SelectList(_tsUtil.BuildProjectList(), "ProjectId", "ProjectName");
            teViewModel.WorkTypes = new SelectList(_tsUtil.BuildWorkTypes(), "WorkTypeId", "WorkTypeName");
            return PartialView("Timesheet/_TimeEntry", teViewModel);
        }

        [HttpPost]
        public void DeleteTimeEntry(int entryId)
        {
            _tsManager.DeleteTimeEntry(entryId);
        }

      
    }
}
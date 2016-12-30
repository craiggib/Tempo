using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TEMPO.BusinessLayer.TimeSheets;
using TEMPO.Data;
using TEMPO.WebApp.Controllers;

namespace TEMPO.WebApp.Controllers
{
    public class AdminController : BaseController
    {
        private TimesheetManager _tsManager;

        public AdminController()
        {
            _tsManager = new TimesheetManager();
        }

        public ActionResult Index()
        {
            Models.Admin.AdminHome adminHomeVm = new Models.Admin.AdminHome();

            adminHomeVm.SubmittedTimesheets = _tsManager
                .GetTimeSheets(new List<TimesheetStatus> { TimesheetStatus.Submitted })
                .Select(i => Mapper.Map<Models.Timesheet.Timesheet>(i))
                .OrderByDescending(i => i.PeriodEnding)
                .ToList();

            return View(adminHomeVm);
        }

        public ActionResult ReviewTimeSheet(int id)
        {
            Models.Timesheet.Timesheet tsViewModel = new TimesheetUtil().GetTimeSheet(id, Mapper, _tsManager);
            return View(tsViewModel);
        }

        [HttpPost]
        public ActionResult Approve(int id)
        {
            _tsManager.SetState(id, TimesheetStatus.Approved);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Reject(int id)
        {
            _tsManager.SetState(id, TimesheetStatus.Rejected);
            return RedirectToAction("Index");
        }

    }
}
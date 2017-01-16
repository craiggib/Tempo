using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TEMPO.BusinessLayer;
using TEMPO.BusinessLayer.TimeSheets;
using TEMPO.Data;
using TEMPO.WebApp.Controllers;

namespace TEMPO.WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : BaseController
    {
        private TimesheetManager _tsManager;
        private TimesheetUtil _tsUtil;
        private AccountManager _accountManager;

        public AdminController()
        {
            _tsManager = new TimesheetManager();
            _accountManager = new AccountManager();
            _tsUtil = new TimesheetUtil(Mapper, _tsManager);
        }

        public ActionResult Index(string employees)
        {

            if (TempData["employee_created"] != null)
            {
                ViewBag.SuccessMessage = "Employee Created";
                TempData.Remove("employee_created");
            }

            Models.Admin.AdminHome adminHomeVm = new Models.Admin.AdminHome();

            adminHomeVm.SubmittedTimesheets = _tsManager
                .GetTimeSheets(new List<TimesheetStatus> { TimesheetStatus.Submitted })
                .Select(i => Mapper.Map<Models.Timesheet.Timesheet>(i))
                .OrderByDescending(i => i.PeriodEnding)
                .ToList();

            adminHomeVm.EmployeeList = _accountManager.GetEmployees()
                .Select(i=>i)
                .ToList()                

                .Select(i=>Mapper.Map<Models.Employee.Employee>(i))
                .OrderBy(i=>i.EmployeeName);

            if(string.IsNullOrEmpty(employees) || employees == "active")
            {
                adminHomeVm.EmployeeList = adminHomeVm.EmployeeList.Where(i => i.Active);                
            }

            return View(adminHomeVm);
        }

        public ActionResult ReviewTimeSheet(int id)
        {
            Models.Timesheet.Timesheet tsViewModel = _tsUtil.GetTimeSheet(id);
            return View(tsViewModel);
        }

        [HttpPost]
        public ActionResult Approve(Models.Timesheet.Timesheet timesheetVm)
        {
            _tsUtil.UpdateTimesheet(timesheetVm);            
            _tsManager.SetState(timesheetVm.TimesheetId, TimesheetStatus.Approved);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Reject(Models.Timesheet.Timesheet timesheetVm)
        {
            if (!string.IsNullOrEmpty(timesheetVm.ApprovalNotes))
            {
                _tsManager.SetApprovalNotes(timesheetVm.TimesheetId, timesheetVm.ApprovalNotes);
            }
            _tsManager.SetState(timesheetVm.TimesheetId, TimesheetStatus.Rejected);
            return RedirectToAction("Index");
        }

            
    }
}
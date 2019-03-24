using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TEMPO.BusinessLayer;
using TEMPO.BusinessLayer.TimeSheets;

namespace TEMPO.WebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EmployeeController : BaseController
    {
        private TimesheetManager _tsManager;
        private TimesheetUtil _tsUtil;
        private AccountManager _accountManager;

        public EmployeeController()
        {
            _tsManager = new TimesheetManager();
            _accountManager = new AccountManager();
            _tsUtil = new TimesheetUtil(Mapper, _tsManager);
        }

        private Models.Employee.Employee GetEmployee(int employeeId, string timesheetFilter)
        {
            var employee = _accountManager.GetEmployee(employeeId);
            if (employee != null)
            {
                Models.Employee.Employee employeeVm = Mapper.Map<Models.Employee.Employee>(employee);

                List<Model.TimeSheet> timeSheets;
                if (string.IsNullOrEmpty(timesheetFilter) || timesheetFilter == "last3months")
                {
                    timeSheets = _tsManager.GetTimeSheets(
                        employeeId,
                        new List<TimesheetStatus>
                        {
                            TimesheetStatus.Approved
                        },
                        DateTime.Now.AddMonths(-3));
                }
                else
                {
                    timeSheets = _tsManager.GetTimeSheets(
                        employeeId,
                        new List<TimesheetStatus>
                        {
                            TimesheetStatus.Approved
                        });
                }

                employeeVm.Timesheets = timeSheets
                    .Select(i => Mapper.Map<Models.Timesheet.Timesheet>(i))
                    .OrderByDescending(i => i.PeriodEnding)
                    .ToList();

                return employeeVm;
            }
            return null;
        }

        public ActionResult Edit(int id, string timesheetFilter)
        {
            Models.Employee.Employee employee = GetEmployee(id, timesheetFilter);

            if (employee == null)
            {
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        [HttpPost]
        public ActionResult Edit(Models.Employee.Employee employeeVm, string timesheetFilter)
        {
            _accountManager.UpdateEmployee(
                employeeVm.EmployeeId,
                employeeVm.EmployeeName,
                employeeVm.Password,
                employeeVm.Rate,
                employeeVm.Active);
            ViewBag.SuccessMessage = "Employee Updated";

            Models.Employee.Employee employee = GetEmployee(employeeVm.EmployeeId, timesheetFilter);
            
            return View(employee);
        }

        [HttpPost]
        public ActionResult Create(Models.Employee.Employee employeeVm)
        {
            _accountManager.CreateEmployee(            
                employeeVm.EmployeeName,
                employeeVm.Password,
                employeeVm.Rate);

            ViewBag.SuccessMessage = "Employee Created";

            TempData["employee_created"] = true;
            return RedirectToAction("Index", "Admin");            
        }
    }
}
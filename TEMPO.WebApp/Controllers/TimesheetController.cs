using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
                foreach(int type in types)
                {
                    statusTypes.Add((TimesheetStatus)type);
                }                
            }

            TimesheetManager tsManager = new TimesheetManager();
            List<TimeSheet> timesheets = tsManager.GetTimeSheets(GetUserID(), statusTypes);

            // convert
            List<Models.Timesheet.Timesheet> timesheetVM = timesheets.Select(i => new Models.Timesheet.Timesheet
            {
                StatusName = i.status.statusname,
                PeriodEnding = i.periodending.endingdate
            }).ToList();

            return View(timesheetVM.OrderByDescending(i=>i.PeriodEnding));
        }
    }
}
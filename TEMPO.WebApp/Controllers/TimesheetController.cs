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
                foreach(int type in types)
                {
                    statusTypes.Add((TimesheetStatus)type);
                }                
            }

            TimesheetManager tsManager = new TimesheetManager();
            List<TimeSheet> timesheets = tsManager.GetTimeSheets(GetUserID(), statusTypes);
                        
            List<Models.Timesheet.Timesheet> timesheetVM = timesheets.Select(i =>
                Mapper.Map<Models.Timesheet.Timesheet>(i)).ToList();            

            return View(timesheetVM.OrderByDescending(i=>i.PeriodEnding));
        }

        [HttpPost]
        public void Create(Models.Timesheet.Timesheet timeSheet)
        {
            var t = timeSheet;
        }


        [HttpPost]
        public void Edit(Models.Timesheet.Timesheet timeSheet)
        {
            var t = timeSheet;
        }

        public ActionResult Edit(int id)
        {
            TimesheetManager tsManager = new TimesheetManager();
            TimeSheet timesheet = tsManager.GetTimeSheet(id);
            Models.Timesheet.Timesheet tsViewModel = Mapper.Map<Models.Timesheet.Timesheet>(timesheet);

            List<Models.Project.Project> projectList = new ProjectManager().GetProjects()                
                .Select(i => Mapper.Map<Models.Project.Project>(i))
                .OrderBy(i=>i.ProjectName)
                .ToList();

            tsViewModel.TimeEntries.ForEach(i =>
            {
                i.Projects = new SelectList(projectList, "ProjectId", "ProjectName", i.ProjectId);
                i.WeeklyTotal = i.Sunday + i.Monday + i.Tuesday + i.Wednesday + i.Thursday + i.Friday + i.Saturday;
            });
            tsViewModel.WeeklyTotal = tsViewModel.TimeEntries.Sum(i => i.WeeklyTotal);
            
            return View(tsViewModel);
        }
    }
}
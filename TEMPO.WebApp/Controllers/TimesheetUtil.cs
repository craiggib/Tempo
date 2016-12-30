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
        public Models.Timesheet.Timesheet GetTimeSheet(int id, IMapper mapper, TimesheetManager timeSheetManager = null)
        {
            TimesheetManager tsManager = timeSheetManager ?? new TimesheetManager();

            TimeSheet timesheet = tsManager.GetTimeSheet(id);
            Models.Timesheet.Timesheet tsViewModel = mapper.Map<Models.Timesheet.Timesheet>(timesheet);
            List<Models.Project.Project> projectList = BuildProjectList(mapper);
            List<Models.Timesheet.WorkType> workTypes = BuildWorkTypes(mapper, tsManager);

            tsViewModel.TimeEntries.ForEach(i =>
            {
                i.Projects = new SelectList(projectList, "ProjectId", "ProjectName", i.ProjectId);
                i.WorkTypes = new SelectList(workTypes, "WorkTypeId", "WorkTypeName", i.WorkTypeId);
            });
            return tsViewModel;
        }

        private List<Models.Project.Project> BuildProjectList(IMapper mapper)
        {
            return new ProjectManager().GetProjects()
                            .Select(i => mapper.Map<Models.Project.Project>(i))
                            .OrderByDescending(i => i.ProjectName)
                            .ToList();
        }

        private List<Models.Timesheet.WorkType> BuildWorkTypes(IMapper mapper, TimesheetManager timeSheetManager)
        {
            return timeSheetManager.GetWorkTypes()
                .Select(i => mapper.Map<Models.Timesheet.WorkType>(i))
                .OrderBy(i => i.WorkTypeName)
                .ToList();
        }
    }
}
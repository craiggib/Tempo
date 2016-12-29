using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TEMPO.Data;

namespace TEMPO.BusinessLayer.TimeSheets
{
    public class TimesheetManager
    {
        public Data.TimeSheet GetTimeSheet(int timeSheetId)
        {
            TempoDbContext dataConext = new TempoDbContext();
            return dataConext.TimeSheets.FirstOrDefault(i => i.tid == timeSheetId);
        }

        public List<Data.TimeSheet> GetTimeSheets(int employeeId, List<TimesheetStatus> status)
        {
            var statusInts = status.Select(i => (int)i);
            TempoDbContext dataConext = new TempoDbContext();
            return dataConext.TimeSheets.Where(i => i.statusid.HasValue && statusInts.Contains(i.statusid.Value) && i.empid == employeeId).ToList();
        }

        public void AddTimeEntry(int timeSheetId, int projectId, int worktypeId, List<DailyTime> dailyWorkTimes)
        {
            TempoDbContext dataConext = new TempoDbContext();
            var timeEntry = new TimeEntry
            {
                sunday = (decimal?)dailyWorkTimes.FirstOrDefault(i => i.DayOfWeek == DayOfWeek.Sunday)?.HoursWorked,
                monday = (decimal?)dailyWorkTimes.FirstOrDefault(i => i.DayOfWeek == DayOfWeek.Monday)?.HoursWorked,
                tuesday = (decimal?)dailyWorkTimes.FirstOrDefault(i => i.DayOfWeek == DayOfWeek.Tuesday)?.HoursWorked,
                wednesday = (decimal?)dailyWorkTimes.FirstOrDefault(i => i.DayOfWeek == DayOfWeek.Wednesday)?.HoursWorked,
                thursday = (decimal?)dailyWorkTimes.FirstOrDefault(i => i.DayOfWeek == DayOfWeek.Thursday)?.HoursWorked,
                friday = (decimal?)dailyWorkTimes.FirstOrDefault(i => i.DayOfWeek == DayOfWeek.Friday)?.HoursWorked,
                saturday = (decimal?)dailyWorkTimes.FirstOrDefault(i => i.DayOfWeek == DayOfWeek.Saturday)?.HoursWorked,
                projectid = projectId,
                tid = timeSheetId,
                worktypeid = worktypeId
            };
            dataConext.TimeEntries.Add(timeEntry);
            dataConext.SaveChanges();
        }

        public void UpdateTimeEntry(int timeEntryId, int projectId, int worktypeId, List<DailyTime> dailyWorkTimes)
        {
            TempoDbContext dataConext = new TempoDbContext();
            var timeEntry = dataConext.TimeEntries.FirstOrDefault(i => i.entryid == timeEntryId);
            if (timeEntry != null)
            {
                timeEntry.sunday = (decimal?)dailyWorkTimes.FirstOrDefault(i => i.DayOfWeek == DayOfWeek.Sunday)?.HoursWorked;
                timeEntry.monday = (decimal?)dailyWorkTimes.FirstOrDefault(i => i.DayOfWeek == DayOfWeek.Monday)?.HoursWorked;
                timeEntry.tuesday = (decimal?)dailyWorkTimes.FirstOrDefault(i => i.DayOfWeek == DayOfWeek.Tuesday)?.HoursWorked;
                timeEntry.wednesday = (decimal?)dailyWorkTimes.FirstOrDefault(i => i.DayOfWeek == DayOfWeek.Wednesday)?.HoursWorked;
                timeEntry.thursday = (decimal?)dailyWorkTimes.FirstOrDefault(i => i.DayOfWeek == DayOfWeek.Thursday)?.HoursWorked;
                timeEntry.friday = (decimal?)dailyWorkTimes.FirstOrDefault(i => i.DayOfWeek == DayOfWeek.Friday)?.HoursWorked;
                timeEntry.saturday = (decimal?)dailyWorkTimes.FirstOrDefault(i => i.DayOfWeek == DayOfWeek.Saturday)?.HoursWorked;
                timeEntry.projectid = projectId;
                timeEntry.worktypeid = worktypeId;
                
                dataConext.SaveChanges();
            }
        }


        public void DeleteTimeEntry(int timeEntryId)
        {
            TempoDbContext dataConext = new TempoDbContext();
            var timeEntry = dataConext.TimeEntries.FirstOrDefault(i => i.entryid == timeEntryId);
            if (timeEntry != null)
            {
                dataConext.TimeEntries.Remove(timeEntry);
                dataConext.SaveChanges();
            }
        }

        public List<Data.WorkType> GetWorkTypes()
        {
            TempoDbContext dataContext = new TempoDbContext();
            return dataContext.WorkTypes.ToList();
        }
    }
}

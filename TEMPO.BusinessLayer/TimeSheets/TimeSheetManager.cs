using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TEMPO.Data;

namespace TEMPO.BusinessLayer.TimeSheets
{
    public class TimesheetManager :BaseManager
    {
        private TempoDbContext _dataContext;

        public TimesheetManager()
        {
            _dataContext = new TempoDbContext();
        }

        #region Timesheets

        public Data.TimeSheet GetTimeSheet(int timeSheetId)
        {
            return _dataContext.TimeSheets.FirstOrDefault(i => i.tid == timeSheetId);
        }

        public List<Data.TimeSheet> GetTimeSheets(int employeeId, List<TimesheetStatus> status)
        {
            var statusInts = status.Select(i => (int)i);
            return _dataContext.TimeSheets.Where(i => i.statusid.HasValue && statusInts.Contains(i.statusid.Value) && i.empid == employeeId).ToList();
        }

        public List<Data.TimeSheet> GetTimeSheets(int employeeId, List<TimesheetStatus> status, DateTime dateFilter)
        {
            var statusInts = status.Select(i => (int)i);
            return _dataContext.TimeSheets
                .Where(i => i.statusid.HasValue && statusInts.Contains(i.statusid.Value) && i.empid == employeeId && i.periodending.endingdate > dateFilter)
                .ToList();
        }

        public List<Data.TimeSheet> GetTimeSheets(List<TimesheetStatus> status)
        {
            var statusInts = status.Select(i => (int)i);
            return _dataContext.TimeSheets
                .Where(i => i.statusid.HasValue && statusInts.Contains(i.statusid.Value))
                .ToList();
        }

        public void SetState(int timeSheetId, TimesheetStatus newState, string notes = null)
        {
            var timeSheet= _dataContext.TimeSheets.FirstOrDefault(i => i.tid == timeSheetId);
            if(timeSheet != null)
            {
                if (notes != null)
                {
                    timeSheet.notes = notes;
                }
                timeSheet.statusid = (int)newState;
                _dataContext.SaveChanges();
            }
        }

        public Data.TimeSheet CreateTimesheet(int employeeId, int periodEndingId)
        {
            Data.TimeSheet newTimeSheet = new Data.TimeSheet
            {
                empid = employeeId,
                peid = periodEndingId,
                statusid = (int)TimesheetStatus.Saved
            };
            _dataContext.TimeSheets.Add(newTimeSheet);
            _dataContext.SaveChanges();
            return newTimeSheet;
        }

        #endregion
        
        #region TimeEntry

        public void AddTimeEntry(int timeSheetId, int projectId, int worktypeId, List<DailyTime> dailyWorkTimes)
        {            
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
            _dataContext.TimeEntries.Add(timeEntry);
            _dataContext.SaveChanges();
        }

        public void UpdateTimeEntry(int timeEntryId, int projectId, int worktypeId, List<DailyTime> dailyWorkTimes)
        {            
            var timeEntry = _dataContext.TimeEntries.FirstOrDefault(i => i.entryid == timeEntryId);
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
                
                _dataContext.SaveChanges();
            }
        }
        
        public void DeleteTimeEntry(int timeEntryId)
        {            
            var timeEntry = _dataContext.TimeEntries.FirstOrDefault(i => i.entryid == timeEntryId);
            if (timeEntry != null)
            {
                _dataContext.TimeEntries.Remove(timeEntry);
                _dataContext.SaveChanges();
            }
        }

        public List<TimeEntry> GetTimeEntries(int projectId)
        {
            return DataContext.TimeEntries.Where(i => i.projectid == projectId)
                .ToList();
        }

        public List<TimeEntrySummary> GetTimeEntrySummaries(int projectId, DateTime? start = null, DateTime? end = null)
        {
            var searchResults = DataContext.TimeEntrySummaries.Where(i => i.projectid == projectId);
            if (start.HasValue && end.HasValue)
            {
                searchResults = searchResults.Where(i => i.endingdate > start.Value && i.endingdate < end.Value);
            }
            return searchResults.ToList();
        }

        #endregion

        #region WorkTypes

        public List<Data.WorkType> GetWorkTypes()
        {            
            return _dataContext.WorkTypes.ToList();
        }

        #endregion

        #region Period Endings

        /// <summary>
        /// return a list of valid period endings. valid = go back and forward one month, remove any existing as options
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public List<Data.PeriodEnding> GetNewPeriodEndings(int employeeId)
        {
            var today = DateTime.Today;
            var forwardOneMonth = today.AddMonths(1);
            var backOneMonth = today.AddMonths(-1);

            List<DateTime> existingPeriods = _dataContext.TimeSheets
                .Where(i => i.empid == employeeId && backOneMonth < i.periodending.endingdate && forwardOneMonth > i.periodending.endingdate)
                .Select(i => i.periodending.endingdate)
                .ToList();

            return _dataContext.PeriodEndings
                .Where(i => backOneMonth < i.endingdate && forwardOneMonth > i.endingdate && !existingPeriods.Contains(i.endingdate))
                .ToList();
        }
        #endregion


    }
}

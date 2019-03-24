using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TEMPO.Model;

namespace TEMPO.BusinessLayer.TimeSheets
{
    public class TimesheetManager : BaseManager
    {

        #region Timesheets

        public Model.TimeSheet GetTimeSheet(int timeSheetId)
        {
            return DataContext.TimeSheets.FirstOrDefault(i => i.tid == timeSheetId);
        }

        public List<Model.TimeSheet> GetTimeSheets(int employeeId, List<TimesheetStatus> status)
        {
            var statusInts = status.Select(i => (int)i);
            return DataContext.TimeSheets.Where(i => i.statusid.HasValue && statusInts.Contains(i.statusid.Value) && i.empid == employeeId).ToList();
        }

        public List<Model.TimeSheet> GetTimeSheets(int employeeId, List<TimesheetStatus> status, DateTime dateFilter)
        {
            var statusInts = status.Select(i => (int)i);
            return DataContext.TimeSheets
                .Where(i => i.statusid.HasValue && statusInts.Contains(i.statusid.Value) && i.empid == employeeId && i.periodending.endingdate > dateFilter)
                .ToList();
        }

        public List<Model.TimeSheet> GetTimeSheets(List<TimesheetStatus> status)
        {
            var statusInts = status.Select(i => (int)i);
            return DataContext.TimeSheets
                .Where(i => i.statusid.HasValue && statusInts.Contains(i.statusid.Value))
                .ToList();
        }

        public void SetState(int timeSheetId, TimesheetStatus newState, string notes = null)
        {
            var timeSheet = DataContext.TimeSheets.FirstOrDefault(i => i.tid == timeSheetId);
            if (timeSheet != null)
            {
                if (notes != null)
                {
                    timeSheet.notes = notes;
                }
                timeSheet.statusid = (int)newState;
                DataContext.SaveChanges();
            }
        }

        public void SetApprovalNotes(int timeSheetId, string notes)
        {
            var timeSheet = DataContext.TimeSheets.FirstOrDefault(i => i.tid == timeSheetId);
            if (timeSheet != null)
            {
                if (notes != null)
                {
                    timeSheet.approvalnotes = notes;
                }
                DataContext.SaveChanges();
            }
        }

        public Model.TimeSheet CreateTimesheet(int employeeId, int periodEndingId)
        {
            Model.TimeSheet newTimeSheet = new Model.TimeSheet
            {
                empid = employeeId,
                peid = periodEndingId,
                statusid = (int)TimesheetStatus.Saved
            };
            DataContext.TimeSheets.Add(newTimeSheet);
            DataContext.SaveChanges();
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
            DataContext.TimeEntries.Add(timeEntry);
            DataContext.SaveChanges();
        }

        public void UpdateTimeEntry(int timeEntryId, int projectId, int worktypeId, List<DailyTime> dailyWorkTimes)
        {
            var timeEntry = DataContext.TimeEntries.FirstOrDefault(i => i.entryid == timeEntryId);
            if (timeEntry != null)
            {
                timeEntry.sunday = TimeOrDefault(dailyWorkTimes, DayOfWeek.Sunday);
                timeEntry.monday = TimeOrDefault(dailyWorkTimes, DayOfWeek.Monday);
                timeEntry.tuesday = TimeOrDefault(dailyWorkTimes, DayOfWeek.Tuesday);
                timeEntry.wednesday = TimeOrDefault(dailyWorkTimes, DayOfWeek.Wednesday);
                timeEntry.thursday = TimeOrDefault(dailyWorkTimes, DayOfWeek.Thursday);
                timeEntry.friday = TimeOrDefault(dailyWorkTimes, DayOfWeek.Friday);
                timeEntry.saturday = TimeOrDefault(dailyWorkTimes, DayOfWeek.Saturday);
                timeEntry.projectid = projectId;
                timeEntry.worktypeid = worktypeId;

                DataContext.SaveChanges();
            }
        }

        private decimal TimeOrDefault(List<DailyTime> dailyWorkTimes, DayOfWeek dayOfWeek)
        {
            var dailyWork = dailyWorkTimes.FirstOrDefault(i => i.DayOfWeek == dayOfWeek);
            return dailyWork == null ? 0 : (decimal)dailyWork.HoursWorked;
        }

        public void DeleteTimeEntry(int timeEntryId)
        {
            var timeEntry = DataContext.TimeEntries.FirstOrDefault(i => i.entryid == timeEntryId);
            if (timeEntry != null)
            {
                DataContext.TimeEntries.Remove(timeEntry);
                DataContext.SaveChanges();
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

        public List<Model.WorkType> GetWorkTypes()
        {
            return DataContext.WorkTypes.ToList();
        }

        #endregion

        #region Period Endings

        /// <summary>
        /// return a list of valid period endings. valid = go back and forward one month, remove any existing as options
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public List<Model.PeriodEnding> GetNewPeriodEndings(int employeeId)
        {
            var today = DateTime.Today;
            var forwardOneMonth = today.AddMonths(1);
            var backOneMonth = today.AddMonths(-1);

            List<DateTime> existingPeriods = DataContext.TimeSheets
                .Where(i => i.empid == employeeId && backOneMonth < i.periodending.endingdate && forwardOneMonth > i.periodending.endingdate)
                .Select(i => i.periodending.endingdate)
                .ToList();

            return DataContext.PeriodEndings
                .Where(i => backOneMonth < i.endingdate && forwardOneMonth > i.endingdate && !existingPeriods.Contains(i.endingdate))
                .ToList();
        }
        #endregion

    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TEMPO.Data;

namespace TEMPO.BusinessLayer.TimeSheets
{
    public class TimesheetManager
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

        public void SetState(int timeSheetId, TimesheetStatus newState, string notes)
        {
            var timeSheet= _dataContext.TimeSheets.FirstOrDefault(i => i.tid == timeSheetId);
            if(timeSheet != null)
            {
                timeSheet.notes = notes;
                timeSheet.statusid = (int)newState;
                _dataContext.SaveChanges();
            }
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

        #endregion

        #region WorkTypes

        public List<Data.WorkType> GetWorkTypes()
        {            
            return _dataContext.WorkTypes.ToList();
        }

        #endregion

    }
}

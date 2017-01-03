CREATE VIEW [dbo].[TimeEntrySummary]
	AS 
select entryid, projectid, sum(sunday+monday+tuesday+wednesday+thursday+friday+saturday) as entryHours, (sum(sunday+monday+tuesday+wednesday+thursday+friday+saturday) * employee.rate) as internalamount,
			employee.employeename, periodending.endingdate, timesheet.tid, worktype.worktypename
from timeentry, timesheet, employee, periodending, worktype
where timeentry.tid = timesheet.tid and employee.empid = timesheet.empid and timesheet.statusid = 3
and timesheet.peid = periodending.peid and timeentry.worktypeid = worktype.worktypeid
group by endingdate, projectid, worktypename, entryid, employee.rate, employeename, timesheet.tid
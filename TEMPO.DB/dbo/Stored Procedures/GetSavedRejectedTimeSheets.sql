
create proc [GetSavedRejectedTimeSheets] 
	@Employeeid int

as

select tid,timesheet.peid,timesheet.empid,timesheet.statusid,notes,endingdate,statusname,employeename,

cast(DATEPART(m, endingdate) as varchar(2)) + '/' 
+ cast(DATEPART(d, endingdate) as varchar (2)) + '/' 
+  cast(DATEPART(yy, endingdate) as varchar(4)) + ' ' 
+ cast (statusname as varchar(9)) as TimeSheetName

from timesheet, status, periodending,employee 
where timesheet.statusid = status.statusid and timesheet.peid = periodending.peid
and employee.empid = timesheet.empid 
and (status.statusname = 'Saved' or status.statusname = 'Rejected') and timesheet.empid = @EmployeeID

select entryid, timeentry.tid, sunday,monday,tuesday,wednesday,thursday,friday,saturday,
worktypeid,timeentry.projectid,client.clientid
from timesheet, status, employee, timeentry, client, project
where timesheet.statusid = status.statusid
and employee.empid = timesheet.empid
and (status.statusname = 'Saved' OR status.statusname = 'Rejected')
and timesheet.empid = @EmployeeID
and timesheet.tid = timeentry.tid
and project.projectid = timeentry.projectid
and project.clientid = client.clientid


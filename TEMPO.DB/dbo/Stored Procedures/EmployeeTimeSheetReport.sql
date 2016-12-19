CREATE  proc [EmployeeTimeSheetReport]
	@tid int

as

SELECT     dbo.timesheet.tid, dbo.timeentry.sunday, dbo.timeentry.monday, dbo.timeentry.tuesday, dbo.timeentry.wednesday, dbo.timeentry.thursday, 
                      dbo.timeentry.friday, dbo.timeentry.saturday, dbo.client.clientname, dbo.project.jobnumyear, dbo.project.refjobnum, dbo.periodending.endingdate, 
                      dbo.employee.employeename, dbo.timeentry.entryid, 
substring(cast(jobyear as char(4)), 3, 2) + '-' + 	
	cast(jobnum as varchar(10) ) + ' ' + cast([description] as varchar(30))
	
	as ProjectName

FROM         dbo.timesheet INNER JOIN
                      dbo.timeentry ON dbo.timesheet.tid = dbo.timeentry.tid INNER JOIN
                      dbo.periodending ON dbo.timesheet.peid = dbo.periodending.peid INNER JOIN
                      dbo.worktype ON dbo.timeentry.worktypeid = dbo.worktype.worktypeid INNER JOIN
                      dbo.project ON dbo.timeentry.projectid = dbo.project.projectid INNER JOIN
                      dbo.client ON dbo.project.clientid = dbo.client.clientid INNER JOIN
                      dbo.employee ON dbo.timesheet.empid = dbo.employee.empid INNER JOIN
			jobyear on project.jobnumyear = jobyear.jobyearid
WHERE     (dbo.timesheet.tid = @tid)

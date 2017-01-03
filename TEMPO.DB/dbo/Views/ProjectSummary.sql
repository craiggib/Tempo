CREATE VIEW [dbo].[ProjectSummary]
	AS 

	select	project.projectid, project.clientid, project.jobnum, project.refjobnum, projecttype.projecttypedesc,
		project.description, project.active, jobyear.JobYear,
		(	select sum([TimeEntrySummary].internalamount) 
			from [TimeEntrySummary]
			where 
					[TimeEntrySummary].projectid = project.projectid				
		) as InternalAmount,
		(	select sum([TimeEntrySummary].entryHours) 
			from [TimeEntrySummary]
			where 
					[TimeEntrySummary].projectid = project.projectid				
		) as TotalHours,
		(
		select top 1 periodending.endingdate 
		from periodending, timeentry, timesheet
		where 
				timesheet.tid = timeentry.tid 
			and timeentry.projectid = project.projectid
			and timesheet.peid = periodending.peid			
		order by periodending.endingdate desc) as lastHoursLogged		
from project, projecttype, jobyear
where project.projecttypeid = projecttype.projecttypeid and project.jobnumyear = jobyear.JobYearID

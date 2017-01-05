CREATE VIEW [dbo].[ProjectSummary]
	AS 

	select	project.projectid, project.clientid, project.jobnum, project.refjobnum, projecttype.projecttypedesc, client.clientname,
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
		order by periodending.endingdate desc) as lastHoursLogged,
		iif(project.projecttypeid = 1, project.contractamount, 
			(select sum([TimeEntrySummary].internalamount) from [TimeEntrySummary] where [TimeEntrySummary].projectid = project.projectid)) as contractamount
		
from project, projecttype, jobyear, client
where project.projecttypeid = projecttype.projecttypeid and project.jobnumyear = jobyear.JobYearID and client.clientid= project.clientid

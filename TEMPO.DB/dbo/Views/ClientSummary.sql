CREATE VIEW [dbo].[ClientSummary]
	AS

SELECT client.clientid, client.clientname, count(project.projectid) as projectcount,
	(select top 1 periodending.endingdate 
		from periodending, timeentry, timesheet, project
		where 
				timesheet.tid = timeentry.tid 
			and timesheet.statusid = 3
			and timeentry.projectid = project.projectid
			and timesheet.peid = periodending.peid
			and	project.clientid = client.clientid
		order by periodending.endingdate desc) as lastHoursLogged,
	(select sum(EntryHours) 
		from [TimeEntrySummary], Project
		where 
				[TimeEntrySummary].projectid = project.projectid
			and project.clientid = client.clientid) as totalhourslogged,
	(select sum([TimeEntrySummary].internalamount) 
		from [TimeEntrySummary], Project
		where 
				[TimeEntrySummary].projectid = project.projectid
			and project.clientid = client.clientid) as internaltotalamount

FROM client left outer join project on client.clientid = project.clientid
--where client.clientid = project.clientid
group by client.clientid, client.clientname
CREATE VIEW [dbo].[ClientSummary]
	AS

SELECT client.clientid, client.clientname, count(project.projectid) as projectcount,
	(select top 1 periodending.endingdate 
		from periodending, timeentry, timesheet, project
		where 
				timesheet.tid = timeentry.tid 
			and timeentry.projectid = project.projectid
			and timesheet.peid = periodending.peid
			and	project.clientid = client.clientid
		order by periodending.endingdate desc) as lastHoursLogged,
	(select sum(EntryHours) 
		from ProjectBillableHours, Project
		where 
				ProjectBillableHours.projectid = project.projectid
			and project.clientid = client.clientid) as totalhourslogged,
	(select sum(amount) 
		from ProjectBillableHours, Project
		where 
				ProjectBillableHours.projectid = project.projectid
			and project.clientid = client.clientid) as totalamount

FROM client, project
where client.clientid = project.clientid
group by client.clientid, client.clientname
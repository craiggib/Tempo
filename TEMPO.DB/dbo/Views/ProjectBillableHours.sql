CREATE VIEW [dbo].[ProjectBillableHours]
	AS 
	select entryid, projectid, sum(sunday+monday+tuesday+wednesday+thursday+friday+saturday) as entryHours, (sum(sunday+monday+tuesday+wednesday+thursday+friday+saturday) * employee.rate) as amount
from timeentry, timesheet, employee
where timeentry.tid = timesheet.tid and employee.empid = timesheet.empid
group by projectid, employee.rate, entryid

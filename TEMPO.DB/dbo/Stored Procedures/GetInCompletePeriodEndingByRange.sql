
create proc [GetInCompletePeriodEndingByRange]
	@From DateTime,
	@To DateTime,
	@Employeeid int
	
as

select PEID, 
cast(DATEPART(m, endingdate) as varchar(2)) + '/' 
	+ cast(DATEPART(d, endingdate) as varchar (2)) + '/' 
	+  cast(DATEPART(yy, endingdate) as varchar(4))
	as EndingDate
 from PeriodEnding where Endingdate between @From and @To
and PEID not in (select PEID from TimeSheet where empid = @EmployeeID)


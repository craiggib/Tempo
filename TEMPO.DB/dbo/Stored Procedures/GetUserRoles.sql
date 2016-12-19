
CREATE proc [GetUserRoles]
@empid int
as
select * from module
select moduleid, empid from moduleauth where empid = @empid


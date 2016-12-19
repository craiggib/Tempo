create proc [DeleteUserRoles]
@empid int
as
delete from moduleauth where empid = @empid


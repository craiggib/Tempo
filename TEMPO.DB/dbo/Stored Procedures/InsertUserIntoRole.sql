
create proc [InsertUserIntoRole]
@empid int, @moduleid int
as 
insert into moduleauth (moduleid, empid) values (@moduleid, @empid)


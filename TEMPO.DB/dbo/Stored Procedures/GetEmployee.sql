
CREATE PROCEDURE [GetEmployee]
	-- Add the parameters for the stored procedure here
	@employeename varchar(255),
	@password varchar(15)
AS
BEGIN
	
	select * from Employee where employeename = @employeename and password = @password;
END

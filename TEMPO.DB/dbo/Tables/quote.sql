CREATE TABLE [dbo].[quote]
(
	[quoteid] INT IDENTITY (100,10) NOT NULL PRIMARY KEY,
	[hours] float not null,
	[description] varchar(500) null,
	[price] decimal(10,2) not null,
	[clientid] int references client(clientid),
	[createddate] datetime not null,
	[lastupdateddate] datetime not null,
	[createdby] int not null references employee(empid),
	[awarded] bit not null default(0)
)

CREATE TABLE [dbo].[quote]
(
	[quoteid] INT IDENTITY (100,10) NOT NULL PRIMARY KEY,
	[hours] float not null,
	[description] varchar(500) null,
	[price] decimal not null,
	[clientid] int references client(clientid)
)

CREATE TABLE [dbo].[quotetags]
(
	[quoteid] INT NOT NULL references quote([quoteid]),
	[title] varchar(100) not null,
	INDEX IX_quotes NONCLUSTERED ([title]),
	INDEX IX_titles NONCLUSTERED ([quoteid]),
	primary key ([quoteid], [title])
)

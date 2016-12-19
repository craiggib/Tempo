CREATE TABLE [dbo].[status] (
    [statusid]   INT       IDENTITY (1, 1) NOT NULL,
    [statusname] CHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([statusid] ASC)
);


CREATE TABLE [dbo].[timesheet] (
    [tid]      INT           IDENTITY (1, 1) NOT NULL,
    [peid]     INT           NULL,
    [empid]    INT           NULL,
    [statusid] INT           NULL,
    [notes]    VARCHAR (500) NULL,
    PRIMARY KEY CLUSTERED ([tid] ASC),
    FOREIGN KEY ([peid]) REFERENCES [dbo].[periodending] ([peid]),
    FOREIGN KEY ([statusid]) REFERENCES [dbo].[status] ([statusid]),
    CONSTRAINT [FK__timesheet__empid__4C6B5938] FOREIGN KEY ([empid]) REFERENCES [dbo].[employee] ([empid])
);


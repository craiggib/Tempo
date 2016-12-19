CREATE TABLE [dbo].[mmt] (
    [mmtid]      INT IDENTITY (1, 1) NOT NULL,
    [entryid]    INT NULL,
    [worktypeid] INT NULL,
    [projectid]  INT NULL,
    PRIMARY KEY CLUSTERED ([mmtid] ASC),
    FOREIGN KEY ([projectid]) REFERENCES [dbo].[project] ([projectid]),
    FOREIGN KEY ([worktypeid]) REFERENCES [dbo].[worktype] ([worktypeid]),
    CONSTRAINT [FK__mmt__entryid__55009F39] FOREIGN KEY ([entryid]) REFERENCES [dbo].[timeentry] ([entryid]) ON DELETE CASCADE
);


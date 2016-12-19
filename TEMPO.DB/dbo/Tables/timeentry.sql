CREATE TABLE [dbo].[timeentry] (
    [entryid]    INT            IDENTITY (1, 1) NOT NULL,
    [tid]        INT            NULL,
    [sunday]     DECIMAL (5, 2) NULL,
    [monday]     DECIMAL (5, 2) NULL,
    [tuesday]    DECIMAL (5, 2) NULL,
    [wednesday]  DECIMAL (5, 2) NULL,
    [thursday]   DECIMAL (5, 2) NULL,
    [friday]     DECIMAL (5, 2) NULL,
    [saturday]   DECIMAL (5, 2) NULL,
    [worktypeid] INT            NULL,
    [projectid]  INT            NULL,
    CONSTRAINT [PK__timeentry__4F47C5E3] PRIMARY KEY CLUSTERED ([entryid] ASC),
    CONSTRAINT [FK__timeentry__proje__5224328E] FOREIGN KEY ([projectid]) REFERENCES [dbo].[project] ([projectid]),
    CONSTRAINT [FK__timeentry__tid__503BEA1C] FOREIGN KEY ([tid]) REFERENCES [dbo].[timesheet] ([tid]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK__timeentry__workt__51300E55] FOREIGN KEY ([worktypeid]) REFERENCES [dbo].[worktype] ([worktypeid])
);


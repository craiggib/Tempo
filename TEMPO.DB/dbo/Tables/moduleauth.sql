CREATE TABLE [dbo].[moduleauth] (
    [empid]    INT NOT NULL,
    [moduleid] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([empid] ASC, [moduleid] ASC),
    FOREIGN KEY ([moduleid]) REFERENCES [dbo].[module] ([moduleid]) ON DELETE CASCADE,
    CONSTRAINT [FK__moduleaut__empid__43D61337] FOREIGN KEY ([empid]) REFERENCES [dbo].[employee] ([empid]) ON DELETE CASCADE
);


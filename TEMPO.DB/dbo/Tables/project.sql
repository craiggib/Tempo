CREATE TABLE [dbo].[project] (
    [projectid]     INT          IDENTITY (1, 1) NOT NULL,
    [clientid]      INT          NULL,
    [jobnumyear]    INT          NOT NULL,
    [jobnum]        VARCHAR (10) NOT NULL,
    [refjobnum]     VARCHAR (50) NULL,
    [projecttypeid] INT          NULL,
    [description]   VARCHAR (30) CONSTRAINT [DF_project_description] DEFAULT (' ') NULL,
    PRIMARY KEY CLUSTERED ([projectid] ASC),
    FOREIGN KEY ([clientid]) REFERENCES [dbo].[client] ([clientid]),
    FOREIGN KEY ([projecttypeid]) REFERENCES [dbo].[projecttype] ([projecttypeid]),
    CONSTRAINT [FK_project_JobYear] FOREIGN KEY ([jobnumyear]) REFERENCES [dbo].[JobYear] ([JobYearID]),
	Active bit not null default(1)
);


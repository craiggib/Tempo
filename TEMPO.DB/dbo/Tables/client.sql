CREATE TABLE [dbo].[client] (
    [clientid]   INT           IDENTITY (1, 1) NOT NULL,
    [clientname] VARCHAR (255) NOT NULL,
    PRIMARY KEY CLUSTERED ([clientid] ASC)
);


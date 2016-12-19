CREATE TABLE [dbo].[employee] (
    [empid]        INT             IDENTITY (1, 1) NOT NULL,
    [employeename] VARCHAR (255)   NOT NULL,
    [password]     VARCHAR (15)    NOT NULL,
    [rate]         DECIMAL (10, 2) NOT NULL,
    [active]       BIT             CONSTRAINT [DF_employee_active] DEFAULT (1) NOT NULL,
    CONSTRAINT [PK__employee__3F115E1A] PRIMARY KEY CLUSTERED ([empid] ASC)
);


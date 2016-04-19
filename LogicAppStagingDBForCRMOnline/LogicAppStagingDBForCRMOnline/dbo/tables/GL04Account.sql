CREATE TABLE [dbo].[GL04Account] (
    [AccountId]        INT              IDENTITY (1, 1) NOT NULL,
    [AccountGuid]      UNIQUEIDENTIFIER NOT NULL,
    [AccountCode]      CHAR (10)        NOT NULL,
    [AccountShortDesc] VARCHAR (20)     NULL,
    [AccountLongDesc]  VARCHAR (50)     NULL,

    CONSTRAINT [PK_AccountCode] PRIMARY KEY CLUSTERED ([AccountCode] ASC)
);

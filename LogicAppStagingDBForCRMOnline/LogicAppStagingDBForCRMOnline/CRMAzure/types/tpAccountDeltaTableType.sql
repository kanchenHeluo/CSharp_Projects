CREATE TYPE [CRMAzure].[tpAccountDeltaTableType] AS TABLE
(
    [AccountId]        INT              NOT NULL,
    [AccountGuid]      UNIQUEIDENTIFIER NOT NULL,
    [AccountCode]      CHAR (10)        NOT NULL,
    [AccountShortDesc] VARCHAR (20)     NULL,
    [AccountLongDesc]  VARCHAR (50)     NULL
)

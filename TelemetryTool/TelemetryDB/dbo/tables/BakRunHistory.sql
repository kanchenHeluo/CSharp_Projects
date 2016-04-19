CREATE TABLE [dbo].[BakRunHistory]
(
	[Id] BIGINT NOT NULL  IDENTITY(1,1),
	[SubscriptionId] UNIQUEIDENTIFIER NOT NULL, 
    [AppName] NCHAR(50) NOT NULL, 
    [Entity] NCHAR(20) NOT NULL, 
    [WorkflowName] NCHAR(50) NOT NULL, 
	[WorkflowTypeId] INT Default 1,
    [Status] NCHAR(20), 
    [StartTime] DATETIME NOT NULL, 
	[EndTime] DATETIME NOT NULL,
    [Duration] Float NOT NULL, 
    [CorrelationId] UNIQUEIDENTIFIER NOT NULL, 
    [RunName] NCHAR(50), 
    [TriggerOutput] NVARCHAR(MAX), 
	[RecordId] NVARCHAR(50) Default '',
    [URL] NVARCHAR(MAX), 
    [CreatedOn] DATETIME NOT NULL Default getutcdate(),
	CONSTRAINT [PK_BakRunHistory] PRIMARY KEY ([Id])
)

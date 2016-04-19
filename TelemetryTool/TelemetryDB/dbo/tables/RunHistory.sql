CREATE TABLE [dbo].[RunHistory]
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
    [CreatedOn] DATETIME Default getutcdate(), 
    CONSTRAINT [PK_RunHistory] PRIMARY KEY ([Id])
)


GO

CREATE NONCLUSTERED INDEX Idx_RH_SAWS
ON [dbo].[RunHistory](WorkflowTypeId)
INCLUDE(SubscriptionId, [AppName], [WorkflowName],[StartTime])

GO

CREATE NONCLUSTERED INDEX [IX_RunHistory_StartTime]
ON [dbo].[RunHistory] ([StartTime])